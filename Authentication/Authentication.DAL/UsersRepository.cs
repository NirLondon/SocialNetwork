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
        private readonly Table _usersTable;
        private readonly Table _tokensTable;

        public UsersRepository()
        {
            //var ddbConfig = new AmazonDynamoDBConfig();
            var client = new AmazonDynamoDBClient(/*ddbConfig*/);
            _usersTable = Table.LoadTable(client, "Users");
            _tokensTable = Table.LoadTable(client, "Tokens");
        }

        public void Signup(UserModel user)
        {
            if (Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else _usersTable.PutItemAsync(Utils.DocumentFrom(user));
        }

        public void SaveToken(TokenModel token)
        {
            _tokensTable.PutItemAsync(Utils.DocumentFrom(token));
        }

        public SignupLoginResult Login(UserModel user)
        {
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            Document result = _usersTable.GetItemAsync(user.UserID).Result;

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
            Document userDoc = _usersTable.GetItemAsync(user.UserID).Result;
            if (userDoc == null)
            {
                userDoc = Utils.DocumentFrom(user);
                _usersTable.PutItemAsync(userDoc);
                eror = SignupLoginResult.EverythingIsGood;
            }
            else if (userDoc["State"] == SignupLoginResult.UserIsBlocked.ToString())
            {
                eror = SignupLoginResult.UserIsBlocked;
            }
            return eror;
        }

        public void BlockUser(string username)
        {
            Document userdoc = _usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserState.Blocked.ToString();
                _usersTable.PutItemAsync(userdoc);
            }
        }

        public void UnBlockUser(string username)
        {
            Document userdoc = _usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserState.Open.ToString();
                _usersTable.PutItemAsync(userdoc);
            }
        }

        public SignupLoginResult SwitchToFacebookUser(string username, string password)
        {
            SignupLoginResult result = SignupLoginResult.WrongUsernameOrPassword;
            Document userDoc = _usersTable.GetItemAsync(username).Result;
            if (userDoc != null && userDoc["Password"] == password)
            {
                result = SignupLoginResult.EverythingIsGood;
                userDoc["Username/Token"] = '_' + username;
                userDoc["Password"] = string.Empty;
                _usersTable.PutItemAsync(userDoc);
            }
            return result;
        }

        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            var userdoc = _usersTable.GetItemAsync(username).Result;
            if (userdoc == null || userdoc["Password"] != oldPassword)
            {
                eror = SignupLoginResult.WrongUsernameOrPassword;
            }
            else
            {
                userdoc["Password"] = newPassword;
                _usersTable.PutItemAsync(userdoc);
            }
            return eror;
        }

        private bool Exists(string userID)
        {
            return _usersTable.GetItemAsync(userID).Result != null;
        }

        public void ExpireToken(string token)
        {
            _tokensTable.PutItemAsync(new Document
            {
                {"Token", token },
                {"IsExpired", true }
            });
        }
    }
}
