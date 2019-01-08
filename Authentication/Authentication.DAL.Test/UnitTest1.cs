using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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
            user["Username/Token"] = "oded";
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            var client = new AmazonDynamoDBClient(ddbConfig);
            Table usersTable = Table.LoadTable(client, "Users");
            var users = usersTable.PutItem(user);
        }

        [TestMethod]
        public void ValueTupleJsonSerialization()
        {
            (string str, int i, DateTime dt) tup = ("aaa", 42, DateTime.Now);
            var json = JsonConvert.SerializeObject(tup);
        }
    }
}
