using Identity.Common.DAL;
using Identity.Common.Models;
using Identity.Common.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Identity.Server.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserDetailsController : ApiController
    {
        private readonly IAuthentiacator _authentiacator;
        private readonly IIdentitiesRepository _repository;
        private readonly INotifier _notifier;

        public UserDetailsController(IAuthentiacator authentiacator, IIdentitiesRepository repository, INotifier notifier)
        {
            _authentiacator = authentiacator;
            _repository = repository;
            _notifier = notifier;
        }

        [HttpPost]
        [Route("Add")]
        public Task<IHttpActionResult> AddUser([FromBody] string userId)
        {
            return WrappedAction(() => _repository.AddUser(userId));
        }

        [HttpPut]
        [Route("EditDetails")]
        public async Task<IHttpActionResult> EditUserDetails([FromBody] UserDetails userDetails)
        {
            if (TryGetToken(out string sentToken))
            {
                var (token, userId) = await _authentiacator.Authenticate(sentToken);
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
                {
                    await _repository.EditUser(userId, userDetails);
                    if (!string.IsNullOrEmpty(userDetails.FirstName) || !string.IsNullOrEmpty(userDetails.LastName))
                    {
                        _notifier.Token = token;
                        _notifier.NotifyToSocialService(new UserToSocial
                        {
                            UserId = userId,
                            FirstName = userDetails.FirstName,
                            LastName = userDetails.LastName
                        });
                    }
                    return WithToken(Ok(), token);
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("GetUserDetails")]
        public Task<IHttpActionResult> GetUserDetails()
        {
            return WrappedAction(userId => _repository.GetUserDetailsAsync(userId));
        }

        [HttpGet]
        [Route("{userId}/Details")]
        public Task<IHttpActionResult> GetUserDetails(string userId)
        {
            return WrappedAction(() => _repository.GetUserDetailsAsync(userId));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, TResult> action)
        {
            return Wrapped(async userId => Json(action(userId)));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, Task<TResult>> action)
        {
            return Wrapped(async userId => Json(await action(userId)));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<Task<TResult>> action)
        {
            return Wrapped(async userId => Json(await action()));
        }

        private Task<IHttpActionResult> WrappedAction(Action action)
        {
            return Wrapped(async str =>
            {
                action();
                return Ok();
            });
        }

        private Task<IHttpActionResult> WrappedAction(Action<string> action)
        {
            return Wrapped(async userId =>
            {
                action(userId);
                return Ok();
            });
        }

        private Task<IHttpActionResult> WrappedAction(Func<string, Task> action)
        {
            return Wrapped(async userId =>
            {
                await action(userId);
                return Ok();
            });
        }

        private async Task<IHttpActionResult> Wrapped(Func<string, Task<IHttpActionResult>> action)
        {
            if (TryGetToken(out string sentToken))
            {
                var (token, userId) = await _authentiacator.Authenticate(sentToken);
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
                    return WithToken(await action(userId), token);
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

        private Tokenned WithToken(IHttpActionResult result, string token)
            => new Tokenned(result, token);
    }
}
