using Authentication.Common.Enums;
using Authentication.Common.Models;
using System;

namespace Authentication.Common.BL
{
    public interface IUsersManager
    {
        Tuple<string, SignupLoginResult> Signup(string username, string password);

        Tuple<string, SignupLoginResult> Login(string username, string password);

        Tuple<string, SignupLoginResult> LoginWithFacebook(string facebookToken);

        SignupLoginResult SwitchToFacebookUser(string username, string password);

        SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword);
    }
}
