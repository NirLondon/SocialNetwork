using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.DAL
{
    public class UserRepository
    {
        AmazonDynamoDBClient client;
        Table usersTable;
        Table tokensTable;
        public UserRepository()
        {
            client = new AmazonDynamoDBClient();
            usersTable = Table.LoadTable(client, "Users");
            tokensTable = Table.LoadTable(client, "Tokens");
        }

        public void Signup(UserModel user)
        {
            if (Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else
            {
                var userDocument = GenerateUserDocument(user);
                usersTable.PutItemAsync(userDocument);
            }
        }

        public void SaveToken(TokenModel token)
        {
            var tokenDocument = GenerateTokenDocument(token);
            tokensTable.PutItemAsync(tokenDocument);
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
                userdoc = GenerateUserDocument(user);
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
            SignupLoginResult eror = SignupLoginResult.WrongUsernameOrPassword;
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null && userdoc["Password"] == password)
            {
                eror = SignupLoginResult.EverythingIsGood;
                userdoc["Username/Token"] = "_" + username;
                userdoc["Password"] = "";
                usersTable.PutItemAsync(userdoc);
            }
            return eror;
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

        private Document GenerateUserDocument(UserModel user)
        {
            var userDocument = new Document();
            userDocument["Username/Token"] = user.UserID;
            userDocument["Password"] = user.Password;
            userDocument["State"] = user.State.ToString();

            return userDocument;
        }

        private Document GenerateTokenDocument(TokenModel token)
        {
            var tokenDocument = new Document();
            tokenDocument["Token"] = token.Token;
            tokenDocument["User"] = token.UserID;
            tokenDocument["CreationDate"] = token.CreationTime;

            return tokenDocument;
        }
    }
}
