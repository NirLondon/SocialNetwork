using Client.Models;
using Client.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Client.Models.ReturnedDTOs;
using Client.Models.UploadedDTOs;

namespace Client.DataProviders
{
    public interface ISocialDataProvider
    {
        Task Post(UploadedPost post);

        Task<RetunredComment> Comment(UploadedComment comment);

        Task<List<ReturnedPost>> GetPosts();

        Task Follow(string userID);

        Task LikePost(Guid postID);

        Task DisLikePost(Guid postID);

        Task Block(string userID);

        Task UnBlock(string userID);

        Task<List<UserMention>> GetBlocked();

        Task<List<UserMention>> GetFollowed();

        Task<List<UserMention>> GetFollowers();

        Task<List<RetunredComment>> GetComments(Guid postID);

        Task UnFollow(string userID);

        Task<UserDetails> Search(string user);
    }
}
