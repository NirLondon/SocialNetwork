using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Authentication.Common.BL;
using Authentication.Common.Enums;

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
                _notifier.Token = token;
                _notifier.NotifyToIdentityService(username);
                _notifier.NotifyToSocailService(username);
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
        [Route("ResetPassword/{username}/{oldPassword}/{newPassword}")]
        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            return _usersManager.ResetPassword(username, oldPassword, newPassword);
        }
    }
}
