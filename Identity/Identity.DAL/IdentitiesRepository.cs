using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Identity.Common.DAL;
using Identity.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.DAL
{
    public class IdentitiesRpository : IIdentitiesRepository
    {
        AmazonDynamoDBClient client;
        Table IdentitiesTable;

        public IdentitiesRpository()
        {
            client = new AmazonDynamoDBClient(
                "AKIAJXYAJT5X7XKWZE4A",
                "eKrd97YbICC8nEZiUqev1UzsfD+Ov9UqFw8YJ1fe",
                RegionEndpoint.USEast2);
            IdentitiesTable = Table.LoadTable(client, "Identities");
        }

        public async void AddUser(string userId)
        {
            var doc = new Document();
            doc["UserID"] = userId;

            await IdentitiesTable.PutItemAsync(doc);
        }

        public async void EditUser(Dictionary<string, object> editedFields)
        {
            await IdentitiesTable.PutItemAsync(Document.FromJson(JsonConvert.SerializeObject(editedFields)));
        }

        public async Task<UserDetails> GetIdentityAsync(string userId)
        {
            var doc = await IdentitiesTable.GetItemAsync(userId);
            return JsonConvert.DeserializeObject<Common.Models.UserDetails>(doc.ToJson());
        }
    }
}
