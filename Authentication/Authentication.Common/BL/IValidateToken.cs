using System.Threading.Tasks;

namespace Authentication.Common.BL
{
    public interface ITokensValidator
    {
        Task<(string Token, string UserId)> ValidateToken(string token);
    }
}
