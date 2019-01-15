using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Social.Common.BL;
using Social.Common.DAL;
using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Social.BL
{
    public class SocialManager : ISocialManager
    {
        private readonly ISocialRepository _repository;

        public SocialManager(ISocialRepository repository)
        {
            _repository = repository;
        }

        public bool AddUser(User user)
        {
            return _repository.AddUser(user);
        }

        public IEnumerable<User> BlockedBy(string userId)
        {
            return _repository.BlockedBy(userId);
        }

        public void Comment(Comment comment)
        {
            _repository.PutComment(comment);
        }

        public IEnumerator<User> GetFollowedBy(string userId)
        {
            return _repository.UsersFollowedBy(userId);
        }

        public IEnumerable<User> GetFollowersOf(string userId)
        {
            return _repository.FollowersOf(userId);
        }

        public IEnumerator<Post> GetPostsFor(string userId, int amount, int skip)
        {
            return _repository.PostsForUser(userId, amount, skip);
        }

        public void Post(string userId, UploaddedPost uploaddedPost)
        {
            string bucketURL = UploadImageToS3(uploaddedPost.Image);
            Post post = GeneratePost(userId, uploaddedPost.Content, bucketURL);
             _repository.PutPost(userId, post);

        }

        public void RemoveFollow(string followerId, int followedId)
        {
            _repository.RemoveFollow(followerId, followedId);
        }

        public IEnumerable<SearchResultUser> Search(string searchedUsername)
        {
            return _repository.Search(searchedUsername);
        }

        public void SetFollow(string followerId, int followedId)
        {
            _repository.SetFollow(followerId, followedId);
        }

        private string UploadImageToS3(byte[] image)
        {
            //string bucketUrl = "";
            string bucketName = "odedbucket";
            var s3Client = new AmazonS3Client(RegionEndpoint.EUWest2);

            using (s3Client)
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = "imageone"
                };
                using (var ms = new MemoryStream(image))
                {
                    request.InputStream = ms;
                    var res = s3Client.PutObjectAsync(request);
                }
            }

            return "bucket url";
        }

        private Post GeneratePost(string userID, string text, string imageURL)
        {
            Post post = new Post
            {
                Comments = new List<Comment>(),
                IMGURL = imageURL,
                PublishDate = DateTime.Now,
                Publisher = userID,
                Text = text
            };
            return post;
        }
    }
}
