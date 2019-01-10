using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Social.Server.Controllers
{
    public class SocialController : ApiController
    {
        const string api = "Social/";

        [HttpPost]
        [Route(api + "GetPosts")]
        public IEnumerable<Post> GetPosts([FromBody]string token)
        {
            
        }

        [HttpPost]
        [Route(api + "PublishPost")]
        public Tuple<string, Post> PublishPost([FromBody](string, string, byte[]) tuple)
        {
            string token = tuple.Item1;
            string text = tuple.Item2;

        }


        [HttpPost]
        [Route(api + "PublishComment")]
        public Comment PublishComment([FromBody]string token, [FromBody]Comment comment)
        {

        }

        [HttpPost]
        [Route(api + "LikePost/{PostID}")]
        public void LikePost([FromBody]string token, int PostID)
        {

        }

        [HttpPost]
        [Route(api + "LikeComment/{CommentID}")]
        public void LikeComment([FromBody]string token, int CommentID)
        {

        }

        [HttpPost]
        [Route(api + "StartFollow/{FollowerID}/{FollowedID}")]
        public void StartFollow([FromBody]string token, int FollowerID, int FollowedID)
        {

        }

        [HttpPost]
        [Route(api + "RemoveFollow/{FollowerID}/{FollowedID}")]
        public void RemoveFollow([FromBody]string token, int FollowerID, int FollowedID)
        {

        }
    }
}