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
        AmazonDynamoDBConfig ddbConfig;
        AmazonDynamoDBClient client;
        Table usersTable;
        Table tokensTable;
        public UserRepository()
        {
            ddbConfig = new AmazonDynamoDBConfig();
            client = new AmazonDynamoDBClient(ddbConfig);
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

        public ErrorEnum Login(UserModel user)
        {
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            Document result = usersTable.GetItemAsync(user.UserID).Result;

            if (result == null || result["Password"] != user.Password)
            {
                eror = ErrorEnum.WrongUsernameOrPassword;
            }
            else if (result["State"] == ErrorEnum.UserIsBlocked.ToString())
            {
                eror = ErrorEnum.UserIsBlocked;
            }
            return eror;
        }

        public ErrorEnum LoginWithFacebook(UserModel user)
        {
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            Document userdoc = usersTable.GetItemAsync(user.UserID).Result;
            if (userdoc == null)
            {
                userdoc = GenerateUserDocument(user);
                usersTable.PutItemAsync(userdoc);
                eror = ErrorEnum.EverythingIsGood;
            }
            else if (userdoc["State"] == ErrorEnum.UserIsBlocked.ToString())
            {
                eror = ErrorEnum.UserIsBlocked;
            }

            return eror;
        }

        public void BlockUser(string username)
        {
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserStateEnum.Blocked.ToString();
                usersTable.PutItemAsync(userdoc);
            }
        }

        public void UnBlockUser(string username)
        {
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null)
            {
                userdoc["State"] = UserStateEnum.Open.ToString();
                usersTable.PutItemAsync(userdoc);
            }
        }

        public ErrorEnum SwitchToFacebookUser(string username, string password)
        {
            ErrorEnum eror = ErrorEnum.WrongUsernameOrPassword;
            Document userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc != null && userdoc["Password"] == password)
            {
                eror = ErrorEnum.EverythingIsGood;
                userdoc["Username/Token"] = "_" + username;
                userdoc["Password"] = "";
                usersTable.PutItemAsync(userdoc);
            }
            return eror;
        }

        public ErrorEnum ResetPassword(string username, string oldPassword, string newPassword)
        {
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            var userdoc = usersTable.GetItemAsync(username).Result;
            if (userdoc == null || userdoc["Password"] != oldPassword)
            {
                eror = ErrorEnum.WrongUsernameOrPassword;
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
            tokenDocument["User"] = token.AssignedUser;
            tokenDocument["State"] = token.State.ToString();
            tokenDocument["CreatedDate"] = DateTime.Now;

            return tokenDocument;
        }
    }
}
