using Authentication.Common.Models;
using System.Threading.Tasks;

namespace Authentication.Common.DAL
{
    public interface ITokensRepository
    {
        TokenModel GetTokenModel(string token);
        Task SaveTokenAsync(TokenModel tokenModel);
    }
}