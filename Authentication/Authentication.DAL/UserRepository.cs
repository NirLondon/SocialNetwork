using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.DAL
{
    public class UserRepository
    {
        AmazonDynamoDBConfig ddbConfig;
        AmazonDynamoDBClient client;
        Table usersTable;
        public UserRepository()
        {
            ddbConfig = new AmazonDynamoDBConfig();
            client = new AmazonDynamoDBClient(ddbConfig);
            usersTable = Table.LoadTable(client, "Users");
        }
        public void Signup(UserModel user)
        {
            if (Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else
            {
                var userDocument = GenerateDocument(user);
                usersTable.PutItemAsync(userDocument);
            }
        }

        private bool Exists(string userID)
        {
            bool flag = true;
            var result = usersTable.GetItemAsync(userID);

            if (result == null)
            {
                flag = false;
            }

            return flag;
        }

        private Document GenerateDocument(UserModel user)
        {
            var userDocument = new Document();
            userDocument["Username/Token"] = user.UserID;
            userDocument["Password"] = user.Password;
            userDocument["State"] = user.State.ToString();

            return userDocument;
        }
    }
}
