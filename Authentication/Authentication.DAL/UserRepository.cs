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
            try
            {
                if (await Exists(user.UserID))
                    throw new UserAlreadyExistsException(user.UserID);
                else
                {
                    var userDocument = GenerateUserDocument(user);
                    await usersTable.PutItemAsync(userDocument);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async void SaveToken(TokenModel token)
        {
            var tokenDocument = GenerateTokenDocument(token);
            await tokensTable.PutItemAsync(tokenDocument);
        }

        private async Task<bool> Exists(string userID)
        {
            Document result;
            bool flag = true;


            result = await usersTable.GetItemAsync(userID);


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
