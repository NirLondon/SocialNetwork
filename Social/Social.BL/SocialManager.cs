using Social.Common.BL;
using Social.Common.DAL;
using Social.Common.Models;
using System.Collections.Generic;

namespace Social.BL
{
    public class SocialManager : ISocialManager
    {
        private readonly ISocialRepository _repository;

        public SocialManager(ISocialRepository repository)
        {
            _repository = repository;
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

        public void Post(string userId, Post post)
        {
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
    }
}
