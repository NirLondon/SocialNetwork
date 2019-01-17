using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Authentication.DAL.Test")]
namespace Authentication.DAL
{
    class Utils
    {
        public static Document DocumentFrom(object obj)
        {
            return Document.FromJson(JsonConvert.SerializeObject(obj));
        }

        public static T FromDocument<T>(Document document) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(document.ToJson());
        }
    }
}
