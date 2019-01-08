using Authentication.Common.Models;

namespace Authentication.Common.DAL
{
    public interface ITokensRepository
    {
        string UserIdOfTokenOrNull(string token);
        TokenModel GetTokenModel(string token);
        string SetNewTokenFor(TokenModel tokenModel);
    }
}