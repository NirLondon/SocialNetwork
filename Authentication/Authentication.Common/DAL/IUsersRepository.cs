using Authentication.Common.Enums;
using Authentication.Common.Models;

namespace Authentication.Common.DAL
{
    public interface IUsersRepository
    {
        void Signup(UserModel user);

        SignupLoginResult Login(UserModel user);

        void SaveToken(TokenModel token);

        SignupLoginResult LoginWithFacebook(UserModel user);

        SignupLoginResult SwitchToFacebookUser(string username, string password);

        SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword);

        void BlockUser(string username);

        void UnBlockUser(string username);
    }
}