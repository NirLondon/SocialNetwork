using System;
using System.Net.Http;
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
            var result = _usersManager.Signup(username, password);
            var eror = result.Item2;
            if (result.Item2 == SignupLoginResult.EverythingIsGood)
            {
                NotifyToIdentityService(result.Item1, username);

            }
            else
                eror = SignupLoginResult.ConectionFailed;
            return new Tuple<string, SignupLoginResult> (result.Item1, eror);
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
        [Route("Logout/{token}")]
        public void Logout(string token)
        {
            _usersManager.ExipreToken(token);
        }

        [HttpGet]
        [Route("ResetPassword/{username}/{oldPassword}/{newPassword}")]
        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            return _usersManager.ResetPassword(username, oldPassword, newPassword);
        }

        private void NotifyToIdentityService(string token, string username)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.PostAsJsonAsync("http://localhost:63276/api/users/editdetails",
                    new
                    {
                        Token = token,
                        EditedDetails = '{' + string.Format(" \"UserID\" : \"{0}\" ", username) + '}'
                    });
            }
        }

        private void NotifyToSocialService(string token, string username)
        {
            throw new NotImplementedException();
        }
    }
}