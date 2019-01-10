using Authentication.Common.Models;

namespace Authentication.Common.DAL
{
    public interface ITokensRepository
    {
        TokenModel GetTokenModel(string token);
        void SaveToken(TokenModel tokenModel);
    }
}