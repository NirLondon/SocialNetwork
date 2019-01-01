using Authentication.Common.BL;
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

        public void SaveToken(TokenModel token)
        {
            repository.SaveToken(token);
        }

        public void Signup(UserModel user)
        {
            repository.Signup(user);
        }
    }
}
