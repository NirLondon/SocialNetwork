using Authentication.Common.BL;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    [RoutePrefix("api/Tokens")]
    public class TokensController : ApiController
    {
        ITokensValidator _validator;

        public TokensController(ITokensValidator validator)
        {
            _validator = validator;
        }

        [HttpGet]
        [Route("Validate/{token}")]
        public (string Token, string UserId) Validate(string token)
        {
            return _validator.ValidateToken(token);
        }
    }
}
