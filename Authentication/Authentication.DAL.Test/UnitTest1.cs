using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using Authentication.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Authentication.DAL.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateUserTest()
        {
            var user = new Document();
            user["UserID"] = "Shahar";
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            var client = new AmazonDynamoDBClient(ddbConfig);
            Table usersTable = Table.LoadTable(client, "Identities");
            var users = usersTable.PutItem(user);
        }

        [TestMethod]
        public void ValueTupleJsonSerialization()
        {
            (string str, int i, DateTime dt) tup = ("aaa", 42, DateTime.Now);
            var json = JsonConvert.SerializeObject(tup);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();

            var tokens = Table.LoadTable(client, "Tokens");

            var res = tokens.GetItemAsync("").Result;
        }

        [TestMethod]
        public void UtilsDocFrom()
        {
            var doc = Utils.DocumentFrom(new TokenModel
            {
                CreationTime = DateTime.Now,
                Token = "12345",
                UserID = "aaa"
            });

            var tok = Utils.FromDocument<TokenModel>(doc);
        }

        [TestMethod]
        public void AddShahar()
        {
            var repo = new TokensRepository();

            repo.SaveToken(new TokenModel
            {
                Token = "this is a token",
                CreationTime = DateTime.Now,
                UserID = "Shahar"
            });

            var shaharDoc = repo.GetTokenModel("this is a token");
        }
    }
}
