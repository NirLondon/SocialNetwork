using Client.Models;
using Client.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.DataProviders
{
    public interface ISocialDataProvider
    {
        Task<(ErrorEnum, Post)> PublishPost(string text, byte[] arr);

        Task<(ErrorEnum, Comment)> PublishComment(string text, byte[] arr);

        Task<(ErrorEnum, List<Post>)> GetPosts();

        Task<(ErrorEnum, UserDetails)> GetUserDetails(string userID);

        Task<ErrorEnum> Follow(string userID);

        Task<ErrorEnum> LikePost(string postID);

        Task<ErrorEnum> Block(string userID);

        Task<(ErrorEnum, List<UserDetails>)> GetFollowed();
    }
}
