using Authentication.Common.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    public class ValidateTokenController : ApiController
    {
        IValidateToken repository;
        const string api = "api/ValidateToken/";

        public ValidateTokenController(IValidateToken manager)
        {
            repository = manager;
        }


        [HttpGet]
        [Route(api + "Validate/{token}")]
        public string Validate(string token)
        {
            string result = repository.ValidateToken(token);
            return result;
        }
    }
}
