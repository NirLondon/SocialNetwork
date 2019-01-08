using System;
using System.Web.Http;
using Authentication.Common.BL;
using Authentication.Common.Enums;

namespace Authentication.Server.Controllers
{
    [RoutePrefix("api/SignupLogin")]
    public class SignupLoginController : ApiController
    {
        private readonly IUsersManager _usersManager;

        public SignupLoginController(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        [HttpGet]
        [Route("Signup/{username}/{password}")]
        public Tuple<string, SignupLoginResult> Signup(string username, string password)
        {
            return _usersManager.Signup(username, password);
        }

        [HttpGet]
        [Route("Login/{username}/{password}")]
        public Tuple<string, SignupLoginResult> Login(string username, string password)
        {
            return _usersManager.Login(username, password);            
        }

        [HttpPost]
        [Route("LoginWithFacebook")]
        public Tuple<string, SignupLoginResult> LoginWithFacebook([FromBody] string facebookToken)
        {
            return _usersManager.LoginWithFacebook(facebookToken);            
        }

        [HttpGet]
        [Route("SwitchToFacebookUser/{username}/{password}")]
        public SignupLoginResult SwitchToFacebookUser(string username, string password)
        {
            return _usersManager.SwitchToFacebookUser(username, password);
        }

        [HttpGet]
        [Route("ResetPassword/{username}/{oldPassword}/{newPassword}")]
        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            return _usersManager.ResetPassword(username, oldPassword, newPassword);
        }
    }
}