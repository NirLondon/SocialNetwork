using Client.Models;
using Client.DataProviders;
using Client.Enum;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.HttpClinents
{
    public class SocialHttpClient : HttpHelper, ISocialDataProvider
    {
        public SocialHttpClient() : base("http://SocialNetwork.Social.com/Social/") { }

        public async Task<ErrorEnum> Follow(string userID)
        {
            var eror = ErrorEnum.ConectionFailed;
            var response = await httpClient.PostAsJsonAsync($"Follow/{userID}", CURRENTTOKEN);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string)>().Result;
                eror = result.Item1;
                CURRENTTOKEN = result.Item2;
            }

            return eror;
        }

        public async Task<(ErrorEnum, List<Post>)> GetPosts()
        {
            var error = ErrorEnum.ConectionFailed;
            List<Post> posts = null;

            var response = await httpClient.PostAsJsonAsync("GetPosts", CURRENTTOKEN);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string, List<Post>)>().Result;
                error = result.Item1;
                CURRENTTOKEN = result.Item2;
                posts = result.Item3;
            }
            var tuple = (error, posts);
            return tuple;
        }

        public async Task<(ErrorEnum, UserDetails)> GetUserDetails(string userID)
        {
            var error = ErrorEnum.ConectionFailed;
            UserDetails userDetails = null;

            var response = await httpClient.PostAsJsonAsync($"PublishPost", (CURRENTTOKEN, userID));

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string, UserDetails)>().Result;
                error = result.Item1;
                CURRENTTOKEN = result.Item2;
                userDetails = result.Item3;
            }
            var tuple = (error, userDetails);
            return tuple;
        }

        public async Task<ErrorEnum> LikePost(string postID)
        {
            var eror = ErrorEnum.ConectionFailed;
            var response = await httpClient.PostAsJsonAsync($"Like/{postID}", CURRENTTOKEN);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string)>().Result;
                eror = result.Item1;
                CURRENTTOKEN = result.Item2;
            }

            return eror;
        }

        public async Task<(ErrorEnum, Comment)> PublishComment(string text, byte[] arr)
        {
            var error = ErrorEnum.ConectionFailed;
            Comment comment = null;

            var response = await httpClient.PostAsJsonAsync($"PublishComment", (CURRENTTOKEN, text, arr));

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string, Comment)>().Result;
                error = result.Item1;
                CURRENTTOKEN = result.Item2;
                comment = result.Item3;
            }
            var tuple = (error, comment);
            return tuple;
        }

        public async Task<(ErrorEnum, Post)> PublishPost(string text, byte[] arr)
        {
            var error = ErrorEnum.ConectionFailed;
            Post post = null;
            
            var response = await httpClient.PostAsJsonAsync($"PublishPost", (CURRENTTOKEN, text, arr));

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string, Post)>().Result;
                error = result.Item1;
                CURRENTTOKEN = result.Item2; 
                post = result.Item3;
            }
            var tuple = (error, post);
            return tuple;
        }

        public async Task<ErrorEnum> Block(string userID)
        {
            var eror = ErrorEnum.ConectionFailed;
            var response = await httpClient.PostAsJsonAsync($"BlockUser/{userID}", CURRENTTOKEN);

            if (response.IsSuccessStatusCode)
            {
                var tuple = response.Content.ReadAsAsync<(ErrorEnum, string)>().Result;
                eror = tuple.Item1;
                CURRENTTOKEN = tuple.Item2;
            }
            return eror;
        }

        public async Task<(ErrorEnum, IEnumerable<UserDetails>)> GetFollowed()
        {
            var eror = ErrorEnum.ConectionFailed;
            IEnumerable<UserDetails> users = new List<UserDetails>();
            var response = await httpClient.PostAsJsonAsync("GetFollowedUsers", CURRENTTOKEN);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, string, IEnumerable<UserDetails>)>().Result;
                eror = result.Item1;
                CURRENTTOKEN = result.Item2;
                users = result.Item3;
            }
            return (eror, users);
        }
    }
}
