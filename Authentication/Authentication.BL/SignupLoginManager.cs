using Authentication.Common.BL;
using Authentication.Common.Models;
using Authentication.DAL;
using System.Threading.Tasks;

namespace Authentication.BL
{
    public class SignupLoginManager : ISignupLogin
    {
        UserRepository repository;

        public SignupLoginManager()
        {
            repository = new UserRepository();
        }

        public void SaveToken(TokenModel token)
        {
            repository.SaveToken(token);
        }

        public Task /*void*/ Signup(UserModel user)
        {
            return repository.Signup(user);
        }
    }
}
