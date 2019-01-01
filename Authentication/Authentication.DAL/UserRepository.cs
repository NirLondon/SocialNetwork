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

        public async void Signup(UserModel user)
        {
            if (await Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else
            {
                var userDocument = GenerateUserDocument(user);
                await usersTable.PutItemAsync(userDocument);
            }
        }

        public async void SaveToken(TokenModel token)
        {

        }

        private async Task<bool> Exists(string userID)
        {
            bool flag = true;
            var result = await usersTable.GetItemAsync(userID);

            if (result == null)
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

        private Document GenerateTokenDocument()
        {

        }
    }
}
