using System.Threading.Tasks;

namespace Identity.Common.BL
{
    public interface IAuthentiacator
    {
        Task<(string Token, string UserId)> Authenticate(string token);
    }
}
