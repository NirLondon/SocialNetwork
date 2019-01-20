using Authentication.Common.BL;
using System;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserStateController : ApiController
    {
        IUserStateManager _manager;

        public UserStateController(IUserStateManager manager)
        {
            _manager = manager;
        }

        [HttpPut]
        [Route("{userId}/Block")]
        public void BlockUser(string userId)
        {
            if (IsAuthorized())
                _manager.BlockUser(userId);
        }

        [HttpPut]
        [Route("{userId}/Unblock")]
        public void UnBlockUser(string userId)
        {
            if (IsAuthorized())
                _manager.UnblockUser(userId);
        }

        private bool IsAuthorized()
        {
            throw new NotImplementedException();
        }
    }
}
