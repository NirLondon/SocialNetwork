using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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

        public bool Login(UserModel user)
        {
            bool flag = true;
            Document result = usersTable.GetItemAsync(user.UserID).Result;

            if (result == null || result["Password"] != user.Password)
            {
                flag = false;
            }

            return flag;
        }

        public bool LoginWithFacebook(UserModel user)
        {
            bool flag = false;
            Document userdoc = usersTable.GetItemAsync(user.UserID).Result;
            if (userdoc == null)
            {
                userdoc = GenerateUserDocument(user);
                usersTable.PutItemAsync(userdoc);
                flag = true;
            }

            return flag;
        }


        private bool Exists(string userID)
        {
            bool flag = true;
            Document userdoc = usersTable.GetItemAsync(userID).Result;

            if (userdoc == null)
            {
                flag = false;
            }

            return flag;
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

            return tokenDocument;
        }
    }
}
