using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.BL
{
    public interface ISignupLogin
    {
        void Signup(UserModel user);

        void SaveToken(TokenModel token);
    }
}
