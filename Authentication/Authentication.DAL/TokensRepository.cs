using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.DAL
{
    public class TokensRepository : ITokensRepository
    {
        AmazonDynamoDBClient _dbClient;
        Table _tokensTable;

        public TokensRepository()
        {
            _dbClient = new AmazonDynamoDBClient(
                "AKIAIJLZ4NP7BCMA5NNQ",
                "UJUULmd66yTN+vV3YQyXjapDh8Z0YlTbaIBb12UH",
                RegionEndpoint.EUWest1);

            _tokensTable = Table.LoadTable(_dbClient, "Tokens");
        }

        public string UserIdOfTokenOrNull(string token)
        {
            return _tokensTable.GetItemAsync(token)
                .Result["User"]
                .AsString();
        }

        public Document GetToken(string token)
        {
            return _tokensTable.GetItemAsync(token).Result;
        }

        public void PutToken(Document doc)
        {
            _tokensTable.PutItemAsync(doc);
        }

        private Document GenerateNewToken(string username)
        {
            Document doc = new Document();
            doc["Token"] = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            doc["AssignedUser"] = username;
            doc["State"] = Common.Enums.TokenStateEnum.Valid.ToString();
            return doc;
        }

        public TokenModel GetTokenModel(string token)
        {
            _tokensTable.GetItemAsync(token).Result;
        }

        public string SetNewTokenFor(TokenModel tokenModel)
        {
            
        }
    }
}
