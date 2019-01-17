//using Authentication.BL;
using Authentication.Common.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    public class UserStateController : ApiController
    {
        IUserState repository;
        const string api = "api/UserState/";

        public UserStateController(IUserState manager)
        {
            repository = manager;
        }


        [HttpGet]
        [Route(api + "BlockUser/{username}")]
        public void BlockUser(string username)
        {
            repository.BlockUser(username);
        }

        [HttpGet]
        [Route(api + "UnBlockUser/{username}")]
        public void UnBlockUser(string username)
        {
            repository.UnblockUser(username);
        }
    }
}
