using Authentication.Common.BL;
using Authentication.Common.Models;
using System;

namespace Authentication.BL
{
    public class SignupLoginUtils : ISignupLoginUtils
    {
        public TokenModel GenerateToken(string username)
        {
            return new TokenModel
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                UserId = username,
                CreationTime = DateTime.Now
            };
        }

        public UserModel GenerateUser(string username, string password)
        {
            return new UserModel
            {
                UserID = username,
                Password = password,
                State = Common.Enums.UserState.Open
            };
        }

        public bool IsValid(string username, string password)
        {
            return
                !string.IsNullOrEmpty(username) &&
                username[0] != '_' &&
                !string.IsNullOrEmpty(password) &&
                password.Length > 4;
        }
    }
}
