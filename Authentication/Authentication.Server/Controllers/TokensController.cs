using Authentication.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    public class TokensController : ApiController
    {
        private readonly ITokensRepository repository;

        public TokensController(ITokensRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("api/tokens/getUserId")]
        public string GetUserId([FromBody] string token)
        {
            return repository.UserIdOfTokenOrNull(token);
        }
    }
}
