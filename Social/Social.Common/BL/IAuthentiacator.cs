using System.Threading.Tasks;

namespace Social.Common.BL
{
    public interface IAuthentiacator
    {
        Task<(string Token, string UserId)> Authenticate(string token);
    }
}
