﻿using Social.Common.BL;
using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Social.Server.Controllers
{
    [RoutePrefix("api/Social")]
    public class SocialController : ApiController
    {
        private readonly IAuthentiacator _authentiacator;
        private readonly ISocialManager _manager;

        public SocialController(IAuthentiacator authentiacator, ISocialManager manager)
        {
            _authentiacator = authentiacator;
            _manager = manager;
        }

        [HttpPost]
        [Route("Users/Add")]
        public Task<IHttpActionResult> AddUser([FromBody] User user)
        {
            return WrappedAction(userId => _manager.AddUser(user));
        }

        [HttpGet]
        [Route("Search/{searchedUsername}")]
        public Task<IHttpActionResult> Search(string searchedUsername)
        {
            return WrappedAction(userId => _manager.Search(searchedUsername));
        }

        [HttpGet]
        [Route("GetPosts/{amount}/{skip}")]
        public Task<IHttpActionResult> GetPosts(int amount, int skip)
        {
            return WrappedAction(userId => _manager.GetPostsFor(userId, amount, skip));
        }

        [HttpPost]
        [Route("PublishPost")]
        public async Task<IHttpActionResult> PublishPostAsync([FromBody] UploaddedPost post)
        {
            var token = ActionContext.Request.Headers.GetValues("Token").FirstOrDefault();
            await _manager.PublishPost(token, post);        
            return await WrappedAction(userId => _manager.PublishPost(userId, post));
        }

        [HttpPost]
        [Route("PublishComment")]
        public Task<IHttpActionResult> PublishComment([FromBody] UploaddedPost comment)
        {
            // return WrappedAction(userid => _manager.Comment(comment));
            return null;
        }

        [HttpPost]
        [Route("LikePost/{PostID}")]
        public Task<IHttpActionResult> LikePost(int postId)
        {
            //return WrappedAction(userId => _manager.(userId, postId));
            return null;
        }

        [HttpPost]
        [Route("Follow/{FollowedId}")]
        public Task<IHttpActionResult> Follow(int FollowedId)
        {
            return WrappedAction(userId => _manager.SetFollow(userId, FollowedId));
        }

        [HttpGet]
        [Route("Unfollow/{FollowedId}")]
        public Task<IHttpActionResult> Unfollow(int FollowedId)
        {
            return WrappedAction(userId => _manager.RemoveFollow(userId, FollowedId));
        }

        [HttpGet]
        [Route("GetFollowedUsers")]
        public Task<IHttpActionResult> GetFollowedUsers()
        {
            return WrappedAction(userId => _manager.GetFollowedBy(userId));
        }

        [HttpGet]
        [Route("Followers")]
        public Task<IHttpActionResult> GetFollowers()
        {
            return WrappedAction(userId => _manager.GetFollowersOf(userId));
        }

        [HttpGet]
        [Route("Blocked")]
        public Task<IHttpActionResult> Blocked()
        {
            return WrappedAction(userId => _manager.BlockedBy(userId));
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
    }
}