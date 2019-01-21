using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Identity.Common.DAL;
using Identity.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.DAL
{
    public class IdentitiesRpository : IIdentitiesRepository
    {
        private readonly Table _identitiesTable;

        public IdentitiesRpository()
        {
            var dbclient = new AmazonDynamoDBClient();

            _identitiesTable = Table.LoadTable(dbclient, "Identities");
        }

        public void AddUser(string userId)
        {
            var doc = new Document();
            doc["UserID"] = userId;
                _identitiesTable.PutItemAsync(doc);
        }

        public async Task EditUser(string userid, UserDetails editedFields)
        {
            var document = Utils.DocumentFrom(editedFields);

            document["UserID"] = userid;

            await _identitiesTable.PutItemAsync(document);
        }

        public async Task<UserDetails> GetUserDetailsAsync(string userId)
        {
            var doc = await _identitiesTable.GetItemAsync(userId);
            doc.Remove("UserID");
            return Utils.FromDocument<UserDetails>(doc);
        }
    }
}
