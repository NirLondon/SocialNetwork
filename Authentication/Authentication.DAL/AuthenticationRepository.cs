using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.DAL
{
    public class AuthenticationRepository
    {
        AmazonDynamoDBConfig ddbConfig;
        AmazonDynamoDBClient client;
        Table tokensTable;

        public AuthenticationRepository()
        {
            ddbConfig = new AmazonDynamoDBConfig();
            client = new AmazonDynamoDBClient(ddbConfig);
            tokensTable = Table.LoadTable(client, "Tokens");
        }


    }
}
