using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Authentication.Common.DAL;
using Authentication.Common.Models;

namespace Authentication.DAL
{
    public class TokensRepository : ITokensRepository
    {
        private readonly Table _tokensTable;

        public TokensRepository()
        {
            var _dbClient = new AmazonDynamoDBClient();

            _tokensTable = Table.LoadTable(_dbClient, "Tokens");
        }

        public TokenModel GetTokenModel(string token)
        {
            var doc = _tokensTable.GetItemAsync(token).Result;
            return doc != null ? Utils.FromDocument<TokenModel>(doc) : null;
        }

        public async void SaveToken(TokenModel tokenModel)
        {
            await _tokensTable.PutItemAsync(Utils.DocumentFrom(tokenModel));
        }
    }
}
