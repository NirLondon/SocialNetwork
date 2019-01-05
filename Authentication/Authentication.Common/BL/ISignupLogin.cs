using Authentication.Common.Enums;
using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.BL
{
    public interface ISignupLogin
    {
        void SaveToken(TokenModel token);

        void Signup(UserModel user);

        ErrorEnum Login(UserModel user);

        ErrorEnum LoginWithFacebook(UserModel user);

        ErrorEnum SwitchToFacebookUser(string username, string password);

        ErrorEnum ResetPassword(string username, string oldPassword, string newPassword);
    }
}
