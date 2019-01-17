using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Social.Common.BL;
using Social.Common.DAL;
using Social.Common.Models;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Social.BL
{
    public class SocialManager : ISocialManager
    {
        private readonly ISocialRepository _repository;
        private readonly IPhotosStorage _photosStorage;

        public SocialManager(ISocialRepository repository, IPhotosStorage photosStorage)
        {
            _repository = repository;
            _photosStorage = photosStorage;
        }

        public void AddUser(User user)
        {
            return _repository.AddUser(user);
        }

        public void Block(string blockingId, string blockedId)
        {
            _repository.Block(blockingId, blockedId);
        }

        public IEnumerable<User> BlockedBy(string userId)
        {
            return _repository.BlockedBy(userId);
        }

        public void Comment(string commenterId, UploadedComment comment)
        {
            _photosStorage.UploadPhoto(comment.Image, out string photoURL);

            _repository.PutComment(commenterId, new DataBaseComment
            {
                Content = comment.Content,
                ImagURL = photoURL,
                PostId = comment.PostId,
                TagedUsersIds = comment.TagedUsersIds
            });
        }

        public IEnumerable<RetunredComment> CommentsOf(string userId, Guid postId)
        {
            return _repository.CommentsOfPost(postId);
        }

        public IEnumerable<User> GetFollowedBy(string userId)
        {
            return _repository.UsersFollowedBy(userId);
        }

        public IEnumerable<User> GetFollowersOf(string userId)
        {
            return _repository.FollowersOf(userId);
        }

        public IEnumerable<ReturnedPost> GetPostsFor(string userId, int amount, int skip)
        {
            return _repository.PostsForUser(userId, amount, skip);
        }

        public void Like(Guid id, string likerId, LikeOptions likeOption)
        {
            _repository.Like(id, likerId, likeOption);
        }

        public void Post(string userId, UploadedPost post)
        {
            _photosStorage.UploadPhoto(post.Image, out string photoURL);

            _repository.PutPost(userId, new DataBasePost
            {
                Content = post.Content,
                ImageURL = photoURL,
                TagedUsersIds = post.TagedUsersIds,
                Visibility = post.Visibility
            });
        }

        public void Remove(Guid id, string uploaderId, RemoveOptions removeOption)
        {
            _repository.Remove(id, uploaderId, removeOption);
        }

        public void RemoveFollow(string followerId, string followedId)
        {
            _repository.RemoveFollow(followerId, followedId);
        }

        public IEnumerable<UserMention> Search(string searchedUsername)
        {
            return _repository.Search(searchedUsername);
        }

        public void SetFollow(string followerId, string followedId)
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

        public void Unblock(string blockingId, string blockedId)
        {
            _repository.Unblock(blockingId, blockedId);
        }

        public void Unlike(Guid id, string likerId, LikeOptions likeOption)
        {
            _repository.Unlike(id, likerId, likeOption);
        }
    }
}
