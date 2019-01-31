using Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Social.Common.Models.ReturnedDTOs;
using Social.Common.Models.UploadedDTOs;

namespace Client.DataProviders
{
    public interface ISocialDataProvider
    {
        Task<ReturnedPost> Post(UploadedPost post);

        Task<RetunredComment> Comment(string text, byte[] arr, string[] tags);

        Task<List<ReturnedPost>> GetPosts();

        Task Follow(string userID);

        Task LikePost(Guid postID);

        Task DisLikePost(Guid postID);

        Task Block(string userID);

        Task UnBlock(string userID);

        Task<List<UserDetails>> GetBlocked();

        Task<List<UserDetails>> GetFollowed();

        Task<List<UserDetails>> GetFollowers();

        Task<List<RetunredComment>> GetComments(Guid postID);

        Task UnFollow(string userID);
    }
}
