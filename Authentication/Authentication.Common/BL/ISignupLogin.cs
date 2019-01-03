using Authentication.Common.Models;
using System.Threading.Tasks;

namespace Authentication.Common.BL
{
    public interface ISignupLogin
    {
        Task /*void*/ Signup(UserModel user);

        void SaveToken(TokenModel token);
    }
}
