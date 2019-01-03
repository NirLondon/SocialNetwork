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

        bool Login(UserModel user);

        bool LoginWithFacebook(UserModel user);
    }
}
