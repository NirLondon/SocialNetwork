using Social.Common.Models;
using System.Collections.Generic;

namespace Social.Common.BL
{
    public interface ISocialManager
    {
        IEnumerable<User> GetFollowersOf(string userId);

        IEnumerator<User> GetFollowedBy(string userId);

        void RemoveFollow(string followerId, int followedId);

        IEnumerator<Post> GetPostsFor(string userId,int amount, int from);

        void SetFollow(string followerId, int followedId);

        void Post(string userId, Post post);

        void Comment(Comment comment);

        IEnumerable<SearchResultUser> Search(string searchedUsername);

        IEnumerable<User> BlockedBy(string userId);
    }
}
