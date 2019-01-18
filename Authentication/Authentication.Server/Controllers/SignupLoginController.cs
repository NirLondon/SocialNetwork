using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Authentication.Common.BL;
using Authentication.Common.Enums;
using Authentication.Server.Models;
using Identity.Common.Models;

namespace Authentication.Server.Controllers
{
    [RoutePrefix("api/SignupLogin")]
    public class SignupLoginController : ApiController
    {
        private readonly IUsersManager _usersManager;
        private readonly INotifier _notifier;

        public SignupLoginController(IUsersManager usersManager, INotifier notifier)
        {
            _usersManager = usersManager;
            _notifier = notifier;
        }

        [HttpGet]
        [Route("Signup/{username}/{password}")]
        public async Task<HttpResponseMessage> Signup(string username, string password)
        {
            var (token, result) = _usersManager.Signup(username, password);
            var response = await Json(result).ExecuteAsync(new CancellationToken());
            if (result == SignupLoginResult.EverythingIsGood)
            {
                _notifier.NotifyToIdentityService()
                await NotifyToIdentityService(token, username);
                response.Headers.Add("Token", token);
            }
            return response;
        }

        [HttpGet]
        [Route("Login/{username}/{password}")]
        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            var (token, result) = _usersManager.Login(username, password);
            var response = await Json(result).ExecuteAsync(new CancellationToken());
            if (result == SignupLoginResult.EverythingIsGood)
                response.Headers.Add("Token", token);
            return response;
        }

        [HttpPost]
        [Route("LoginWithFacebook")]
        public async Task<HttpResponseMessage> LoginWithFacebook([FromBody] string facebookToken)
        {
            var (token, result) = _usersManager.LoginWithFacebook(facebookToken);
            var response = await Json(result).ExecuteAsync(new CancellationToken());
            if (result == SignupLoginResult.EverythingIsGood)
                response.Headers.Add("Token", token);
            return response;
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

        private async Task NotifyToIdentityService(string token, string username)
        {
            var details = new UserDetails { UserId = username };
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Token", token);
            await client.PutAsJsonAsync("http://localhost:63276/api/users/editdetails", details);
            //http://localhost:63276/
            //http://SocialNetwork.Identity.com
        }

        private void NotifyToSocialService(string token, string username)
        {
            throw new NotImplementedException();
        }
    }
}
