using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Social.Common.BL;
using Social.Common.DAL;
using Social.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        public async void PublishComment(string userId, UploaddedPost uploaddedPost)
        {
            string bucketURL = await UploadImageToS3(uploaddedPost.Image);
            Comment comment = GenerateComment(userId, uploaddedPost.Content, bucketURL);
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

        public async Task PublishPost(string userId, UploaddedPost uploaddedPost)
        {
            string bucketURL = await UploadImageToS3(uploaddedPost.Image);
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

        private async Task<string> UploadImageToS3(byte[] image)
        {
            string bucketUrl = "https://s3-eu-west-1.amazonaws.com/odedbucket/";
            string bucketName = "odedbucket";
            string key = GetNewToken();
            var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1);
            using (s3Client)
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = key
                };
                using (var ms = new MemoryStream(image))
                {
                    request.InputStream = ms;
                    await s3Client.PutObjectAsync(request);                  
                }
            }

            return bucketUrl + key;
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

        private Comment GenerateComment(string UserId, string text, string imageURL)
        {
            Comment comment = new Comment
            {
                Content = text,
                Publisher = UserId,
                PublishDate = DateTime.Now,
                IMGURL = imageURL
            };
            return comment;
        }

        public static string GetNewToken()
        {
            byte[] arr = new byte[16];
            new Random().NextBytes(arr);
            return Convert.ToBase64String(arr);
        }
    }
}
