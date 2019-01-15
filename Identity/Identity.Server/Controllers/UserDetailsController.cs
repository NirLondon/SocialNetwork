using Identity.Common.DAL;
using Identity.Common.Models;
using Identity.Server.Models;
using Identity.Common.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Identity.Server.Controllers
{
    public class UserDetailsController : ApiController
    {
        private readonly IAuthentiacator _authentiacator;
        private readonly IIdentitiesRepository _repository;

        public UserDetailsController(IAuthentiacator authentiacator, IIdentitiesRepository repository)
        {
            _authentiacator = authentiacator;
            _repository = repository;
        }

        [HttpPut]
        [Route("api/users/EditDetails")]
        public Task<IHttpActionResult> EditUserDetails([FromBody] UserDetails userDetails)
        {
            return WrappedAction(async userId => await _repository.EditUser(userId, userDetails));
        }

        [HttpGet]
        [Route("api/Users/GetUserDetails")]
        public Task<IHttpActionResult> GetUserDetails()
        {
            return WrappedAction(userId => _repository.GetUserDetailsAsync(userId));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, TResult> action)
        {
            return WrappedAction(userId => Json(action(userId)));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, Task<TResult>> action)
        {
            return WrappedAction(async userId => Json(await action(userId)));
        }

        private Task<IHttpActionResult> WrappedAction(Action<string> action)
        {
            return WrappedAction(userId =>
            {
                action(userId);
                return Ok();
            });
        }

        private Task<IHttpActionResult> WrappedAction(Func<string, Task> action)
        {
            return WrappedAction(async userId =>
            {
                await action(userId);
                return Ok();
            });
        }

        private async Task<IHttpActionResult> WrappedAction(Func<string, Task<IHttpActionResult>> action)
        {
            if (TryGetToken(out string sentToken))
            {
                var (token, userId) = await _authentiacator.Authenticate(sentToken);
                if (token != null && userId != null)
                    return await action(userId);
            }
            return Unauthorized();
        }

        private bool TryGetToken(out string sentToken)
        {
            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> res))
            {
                sentToken = res.FirstOrDefault();
                return !string.IsNullOrEmpty(sentToken);
            }
            sentToken = null;
            return false;
        }

        //[HttpGet]
        //[Route("api/users/details/{token}")]
        //public async Task<(string Token, UserDetails)> GetUserDetails(string token)
        //{
        //    var tokenUser = await GetUserIdAndTokenFromToken(token);

        //    if (tokenUser.UserId != null)
        //        return (tokenUser.Token, await repository.GetUserDetailsAsync(tokenUser.UserId));
        //    return (null, null);
        //}

        //private async Task<(string Token, string UserId)> GetUserIdAndTokenFromToken(string token)
        //{
        //    var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:63172/") };
        //    var response = await httpClient.GetAsync($"api/Tokens/Validate/{token}");

        //    if (response.IsSuccessStatusCode)
        //        return response.Content.ReadAsAsync<(string, string)>().Result;

        //    throw new Exception("The authentication service is not available. ");
        //}
    }
}
