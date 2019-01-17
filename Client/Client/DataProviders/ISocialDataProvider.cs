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
        Task<Post> PublishPost(string text, byte[] arr, string[] tags);

        Task<Comment> PublishComment(string text, byte[] arr, string[] tags);

        Task<List<Post>> GetPosts();

        Task Follow(string userID);

        Task LikePost(string postID);

        Task Block(string userID);

        Task<IEnumerable<UserDetails>> GetFollowed();
    }
}
