using Authentication.Common.BL;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    public class TokensController : ApiController
    {
        ITokensValidator _validator;
        const string api = "api/Tokens/";

        public TokensController(ITokensValidator validator)
        {
            _validator = validator;
        }


        [HttpGet]
        [Route(api + "Validate/{token}")]
        public (string Token, string UserId) Validate(string token)
        {
            return _validator.ValidateToken(token);
        }
    }
}
