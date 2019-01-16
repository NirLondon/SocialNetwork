using Social.Common.BL;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.UploadedDTOs;
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
        [Route("Post")]
        public Task<IHttpActionResult> Post([FromBody] UploadedPost post)
        {
            return WrappedAction(userId => _manager.Post(userId, post));
        }

        [HttpPost]
        [Route("Comment")]
        public Task<IHttpActionResult> Comment([FromBody] UploadedComment comment)
        {
            return WrappedAction(userId => _manager.Comment(userId, comment));
        }

        [HttpGet]
        [Route("CommentsOf")]
        public Task<IHttpActionResult> CommentsOf(Guid postId)
        {
            return WrappedAction(userId => _manager.CommentsOf(userId, postId));
        }

        [HttpPost]
        [Route("LikePost/{postId}")]
        public Task<IHttpActionResult> LikePost(Guid postId)
        {
            return WrappedAction(userId => _manager.Like(postId, userId, LikeOptions.Post));
        }

        [HttpPost]
        [Route("LikeComment/{commentId}")]
        public Task<IHttpActionResult> LikeComment(Guid commentId)
        {
            return WrappedAction(userId => _manager.Like(CommentId, userId, LikeOptions.Comment));
        }

        [HttpDelete]
        [Route("UnlikePost/{postId}")]
        public Task<IHttpActionResult> UnlikePost(Guid postId)
        {
            return WrappedAction(userId => _manager.Unlike(postId, userId, LikeOptions.Post));
        }

        [HttpDelete]
        [Route("UnlikeComment/{commentId}")]
        public Task<IHttpActionResult> UnlikeComment(Guid commentId)
        {
            return WrappedAction(userId => _manager.Unlike(commentId, userId, LikeOptions.Post));
        }

        [HttpPost]
        [Route("Follow/{FollowedId}")]
        public Task<IHttpActionResult> Follow(string FollowedId)
        {
            return WrappedAction(userId => _manager.SetFollow(userId, FollowedId));
        }

        [HttpDelete]
        [Route("Unfollow/{FollowedId}")]
        public Task<IHttpActionResult> Unfollow(string FollowedId)
        {
            return WrappedAction(userId => _manager.RemoveFollow(userId, FollowedId));
        }

        [HttpPost]
        [Route("Followed")]
        public Task<IHttpActionResult> GetFollowed(string token)
        {
            return WrappedAction(userId => _manager.GetFollowedBy(userId));
        }

        [HttpGet]
        [Route("Followers")]
        public Task<IHttpActionResult> GetFollowers()
        {
            return WrappedAction(userId => _manager.GetFollowersOf(userId));
        }

        [HttpPost]
        [Route("Block")]
        public Task<IHttpActionResult> Block(string blockedId)
        {
            return WrappedAction(userId => _manager.Block(userId, blockedId));
        }

        [HttpDelete]
        [Route("Unblock")]
        public Task<IHttpActionResult> Unblock(string blockedId)
        {
            return WrappedAction(userId => _manager.Unblock(userId, blockedId));
        }
        
        [HttpGet]
        [Route("Blocked")]
        public Task<IHttpActionResult> Blocked()
        {
            return WrappedAction(userId => _manager.BlockedBy(userId));
        }

        [HttpDelete]
        [Route("RemovePost/{postId}")]
        public Task<IHttpActionResult> RemovePost(Guid postId)
        {
            return WrappedAction(userId => _manager.Remove(postId, userId, RemoveOptions.Post));
        }

        [HttpDelete]
        [Route("RemoveComment/{commentId}")]
        public Task<IHttpActionResult> RemoveComment(Guid commentId)
        {
            return WrappedAction(userId => _manager.Remove(commentId, userId, RemoveOptions.Comment));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, TResult> action)
        {
            return Wrapped(async userId => await Task.FromResult(Json(action(userId))));
        }

        private Task<IHttpActionResult> WrappedAction<TResult>(Func<string, Task<TResult>> action)
        {
            return Wrapped(async userId => Json(await action(userId)));
        }

        private Task<IHttpActionResult> WrappedAction(Action<string> action)
        {
            return Wrapped(async userId =>
            {
                action(userId);
                return await Task.FromResult(Ok());
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