using Authentication.Common.BL;
using Authentication.Common.DAL;
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
                    return (_repository.SetNewTokenFor(tokenModel), tokenModel.UserId);
                }
                return (token, tokenModel.UserId);
            }
            return (null, null);
        }
    }
}
