using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.DAL
{
    public class TokensRepository : ITokensRepository
    {
        AmazonDynamoDBClient client;
        Table tokensTable;

        public TokensRepository()
        {
            client = new AmazonDynamoDBClient(
                "AKIAIJLZ4NP7BCMA5NNQ",
                "UJUULmd66yTN+vV3YQyXjapDh8Z0YlTbaIBb12UH",
                RegionEndpoint.EUWest1);

            tokensTable = Table.LoadTable(client, "Tokens");
        }

        public bool Exists(string token)
        {
            throw new NotImplementedException();
        }

        public string UserIdOfTokenOrNull(string token)
        {
            return tokensTable.GetItemAsync(token)
                .Result["UserID"]
                .AsString();
        }
    }
}
