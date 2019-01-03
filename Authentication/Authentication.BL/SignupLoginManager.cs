using Authentication.Common.BL;
using Authentication.Common.Enums;
using Authentication.Common.Models;
using Authentication.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.BL
{
    public class SignupLoginManager : ISignupLogin
    {
        UserRepository repository;

        public SignupLoginManager()
        {
            repository = new UserRepository();
        }

        public ErrorEnum Login(UserModel user)
        {
            return repository.Login(user);
        }

        public ErrorEnum LoginWithFacebook(UserModel user)
        {
            return repository.LoginWithFacebook(user);
        }

        public ErrorEnum ResetPassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void SaveToken(TokenModel token)
        {
            repository.SaveToken(token);
        }

        public void Signup(UserModel user)
        {
            repository.Signup(user);
        }

        public ErrorEnum SwitchToFacebookUser(string username, string passwrod)
        {
            var eror = repository.SwitchToFacebookUser(username, passwrod);
            return eror;
        }
    }
}
