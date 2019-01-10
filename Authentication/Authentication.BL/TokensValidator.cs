using Authentication.Common.BL;
using Authentication.Common.DAL;
using Authentication.Common.Models;
using System;

namespace Authentication.BL
{
    public class TokensValidator : ITokensValidator
    {
        private int tokenExpirationMniutes { get; set; }
        ITokensRepository _repository;

        public TokensValidator(ITokensRepository repository)
        {
            tokenExpirationMniutes = 15;
            _repository = repository;
        }

        public (string Token, string UserId) ValidateToken(string token)
        {
            var tokenModel = _repository.GetTokenModel(token);

            if (tokenModel != null)
            {
                var now = DateTime.Now;
                if (now - tokenModel.CreationTime > TimeSpan.FromMinutes(15))
                {
                    if (now - tokenModel.CreationTime > TimeSpan.FromMinutes(30))
                        return (null, null);
                    tokenModel = new TokenModel
                    {
                        Token = Utils.GetNewToken(),
                        CreationTime = DateTime.Now,
                        UserID = tokenModel.UserID
                    };
                    _repository.SaveToken(tokenModel);
                    return (tokenModel.Token, tokenModel.UserID);
                }
                return (token, tokenModel.UserID);
            }
            return (null, null);
        }
    }
}
