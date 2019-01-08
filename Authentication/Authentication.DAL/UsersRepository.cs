using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;

namespace Authentication.DAL
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Table usersTable;
        private readonly Table tokensTable;

        public UsersRepository()
        {
            var ddbConfig = new AmazonDynamoDBConfig();
            var client = new AmazonDynamoDBClient(ddbConfig);
            usersTable = Table.LoadTable(client, "Users");
            tokensTable = Table.LoadTable(client, "Tokens");
        }

        public void Signup(UserModel user)
        {
            if (Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else usersTable.PutItemAsync(DocumentFrom(user));
        }

        public void SaveToken(TokenModel token)
        {
            tokensTable.PutItemAsync(DocumentFrom(token));
        }

        public SignupLoginResult Login(UserModel user)
        {
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            Document result = usersTable.GetItemAsync(user.UserID).Result;

            if (result == null || result["Password"] != user.Password)
            {
                eror = SignupLoginResult.WrongUsernameOrPassword;
            }
            else if (result["State"] == SignupLoginResult.UserIsBlocked.ToString())
            {
                eror = SignupLoginResult.UserIsBlocked;
            }
            return eror;
        }

        public SignupLoginResult LoginWithFacebook(UserModel user)
        {
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            Document userdoc = usersTable.GetItemAsync(user.UserID).Result;
            if (userdoc == null)
            {
                userdoc = DocumentFrom(user);
                usersTable.PutItemAsync(userdoc);
                eror = SignupLoginResult.EverythingIsGood;
            }
            else if (userdoc["State"] == SignupLoginResult.UserIsBlocked.ToString())
            {
                eror = SignupLoginResult.UserIsBlocked;
            }

            return eror;
        }

        public void BlockUser(string username)
        {
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserState.Blocked.ToString();
                usersTable.PutItemAsync(userdoc);
            }
        }

        public void UnBlockUser(string username)
        {
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserState.Open.ToString();
                usersTable.PutItemAsync(userdoc);
            }
        }

        public SignupLoginResult SwitchToFacebookUser(string username, string password)
        {
            SignupLoginResult result = SignupLoginResult.WrongUsernameOrPassword;
            Document userDoc = usersTable.GetItemAsync(username).Result;
            if (userDoc != null && userDoc["Password"] == password)
            {
                result = SignupLoginResult.EverythingIsGood;
                userDoc["Username/Token"] = '_' + username;
                userDoc["Password"] = string.Empty;
                usersTable.PutItemAsync(userDoc);
            }
            return result;
        }

        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            var userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc == null || userdoc["Password"] != oldPassword)
            {
                eror = SignupLoginResult.WrongUsernameOrPassword;
            }
            else
            {
                userdoc["Password"] = newPassword;
                usersTable.PutItemAsync(userdoc);
            }
            return eror;
        }

        private bool Exists(string userID)
        {
            return usersTable.GetItemAsync(userID).Result != null;
        }

        private Document DocumentFrom(UserModel user)
        {
            return new Document
            {
                { "Username/Token", user.UserID },
                { "Password", user.Password },
                { "State", user.State.ToString() }
            };
        }

        private Document DocumentFrom(TokenModel token)
        {
            return new Document
            {
                { "Token", token.Token },
                { "UserID", token.UserId },
                { "CreationDate", token.CreationTime }
            };
        }
    }
}
