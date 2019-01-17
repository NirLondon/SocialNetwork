using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Social.Common.BL
{
    public interface ISocialManager
    {
        IEnumerable<User> GetFollowersOf(string userId);

        IEnumerable<User> GetFollowedBy(string userId);

        void RemoveFollow(string followerId, string followedId);

        IEnumerable<ReturnedPost> GetPostsFor(string userId,int amount, int from);

        void SetFollow(string followerId, string followedId);

        void Post(string userId, UploadedPost post);

        void Comment(string commenterId, UploadedComment comment);

        IEnumerable<UserMention> Search(string searchedUsername);

        IEnumerable<User> BlockedBy(string userId);

        void AddUser(User user);

        void Block(string userId, string blockedId);

        void Unblock(string userId, string blockedId);

        IEnumerable<RetunredComment> CommentsOf(string userId, Guid postId);

        void Like(Guid id, string likerId, LikeOptions likeOption);

        void Unlike(Guid id, string likerId, LikeOptions likeOption);

        void Remove(Guid id, string uploaderId, RemoveOptions removeOption);
    }
}
