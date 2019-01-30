using System;
using System.Collections.Generic;
using Social.Common.Models;
using Social.Common.Models.DataBaseDTOs;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;

namespace Social.Common.DAL
{
    public interface ISocialRepository
    {
        void SetFollow(string followerId, string followedId);

        IEnumerable<UserMention> BlockedBy(string userId);

        void PutComment(string commenterId, DataBaseComment comment);

        IEnumerable<UserMention> UsersFollowedBy(string userId);

        IEnumerable<UserMention> FollowersOf(string userId);

        IEnumerable<ReturnedPost> PostsForUser(string userId, int amount, int skip);

        void AddUser(string userId);

        ReturnedPost PutPost(string userId, DataBasePost post);

        void Block(string blockingId, string blockedId);

        void RemoveFollow(string followerId, string followedId);

        IEnumerable<UserMention> Search(string searchedUsername);

        void Unblock(string blockingId, string blockedId);

        IEnumerable<RetunredComment> CommentsOfPost(Guid postId, string userId);

        void Like(Guid id, string likerId, LikeOptions likeOption);

        void Unlike(Guid id, string likerId, LikeOptions likeOption);

        void Remove(Guid id, string uploaderId, RemoveOptions removeOption);

        void EditUser(User user);
    }
}
