﻿using Authentication.Common.Enums;

namespace Authentication.Common.BL
{
    public interface IUsersManager
    {
        (string token, SignupLoginResult) Signup(string username, string password);

        (string token, SignupLoginResult) Login(string username, string password);

        (string token, SignupLoginResult) LoginWithFacebook(string facebookToken);

        SignupLoginResult SwitchToFacebookUser(string username, string password);

        SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword);

        void ExipreToken(string token);
    }
}
