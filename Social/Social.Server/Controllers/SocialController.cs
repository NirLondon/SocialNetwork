using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Social.Server.Controllers
{
    [RoutePrefix("api/Social")]
    public class SocialController : ApiController
    {

        [HttpPost]
        [Route("GetPosts")]
        public IEnumerable<Post> GetPosts([FromBody]string token)
        {
            
        }

        [HttpPost]
        [Route("PublishPost")]
        public Post PublishPost([FromBody]string token, [FromBody]Post post)
        {

        }

        [HttpPost]
        [Route("PublishComment")]
        public Comment PublishComment([FromBody]string token, [FromBody]Comment comment)
        {

        }

        [HttpPost]
        [Route("LikePost/{PostID}")]
        public void LikePost([FromBody]string token, int PostID)
        {

        }

        [HttpPost]
        [Route("LikeComment/{CommentID}")]
        public void LikeComment([FromBody]string token, int CommentID)
        {

        }

        [HttpPost]
        [Route("StartFollow/{FollowerID}/{FollowedID}")]
        public void StartFollow([FromBody]string token, int FollowerID, int FollowedID)
        {

        }

        [HttpPost]
        [Route("RemoveFollow/{FollowerID}/{FollowedID}")]
        public void RemoveFollow([FromBody]string token, int FollowerID, int FollowedID)
        {

        }
    }
}