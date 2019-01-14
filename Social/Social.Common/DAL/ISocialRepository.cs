using System.Collections.Generic;
using Social.Common.Models;

namespace Social.Common.DAL
{
    public interface ISocialRepository
    {
        void SetFollow(string followerId, string followedId);

        IEnumerable<User> BlockedBy(string userId);

        void PutComment(Comment comment);

        IEnumerator<User> UsersFollowedBy(string userId);

        IEnumerable<User> FollowersOf(string userId);

        IEnumerator<Post> PostsForUser(string userId, int amount, int skip);

        bool AddUser(User user);

        void PutPost(string userId, Post post);

        void RemoveFollow(string followerId, string followedId);

        IEnumerable<SearchResultUser> Search(string searchedUsername);
    }
}
