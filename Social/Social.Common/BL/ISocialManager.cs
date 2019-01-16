using Social.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Social.Common.BL
{
    public interface ISocialManager
    {
        IEnumerable<User> GetFollowersOf(string userId);

        IEnumerator<User> GetFollowedBy(string userId);

        void RemoveFollow(string followerId, int followedId);

        IEnumerator<Post> GetPostsFor(string userId,int amount, int from);

        void SetFollow(string followerId, int followedId);

        Task PublishPost(string userId, UploaddedPost post);

        void PublishComment(Comment comment);

        IEnumerable<SearchResultUser> Search(string searchedUsername);

        IEnumerable<User> BlockedBy(string userId);

        bool AddUser(User user);
    }
}
