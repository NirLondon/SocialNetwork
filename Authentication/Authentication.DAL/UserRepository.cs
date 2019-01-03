using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using System.Threading.Tasks;

namespace Authentication.DAL
{
    public class UserRepository : IUsersRepository
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

        public async Task /*void*/ Signup(UserModel user)
        {
            if (await Exists(user.UserID))
                throw new UserAlreadyExistsException(user.UserID);
            else
            {
                var userDocument = GenerateUserDocument(user);
                await usersTable.PutItemAsync(userDocument)/*.Wait()*/;
            }
        }

        public void SaveToken(TokenModel token)
        {
            var tokenDocument = GenerateTokenDocument(token);
            tokensTable.PutItemAsync(tokenDocument);
        }

        private async Task<bool> /*bool*/ Exists(string userID)
        {
            return await usersTable.GetItemAsync(userID) != null;
            //return usersTable.GetItemAsync(userID).Result != null;
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
