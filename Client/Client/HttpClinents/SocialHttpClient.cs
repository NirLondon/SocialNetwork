using Client.Models;
using Client.DataProviders;
using Client.Enum;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Net;
using Client.Exeptions;

namespace Client.HttpClinents
{
    public class SocialHttpClient : HttpHelper, ISocialDataProvider
    {
        private int GetPostIndex { get; set; } = 0;
        private int SkipPostAmount { get; set; } = 5;

        public SocialHttpClient() : base("http://localhost:63377/api/Social/") { }
        //http://localhost:63377/
        //http://SocialNetwork.Social.com/

        public async Task Follow(string userID)
        {
            var response = await httpClient.GetAsync($"Follow/{userID}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<List<Post>> GetPosts()
        {
            var response = await httpClient.GetAsync($"GetPosts/{SkipPostAmount}/{GetPostIndex}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return await response.Content.ReadAsAsync<List<Post>>();
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task LikePost(string postID)
        {
            var response = await httpClient.GetAsync($"Like/{postID}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<Comment> PublishComment(string text, byte[] arr, string[] tags)
        {
            var response = await httpClient.PostAsJsonAsync($"PublishComment", new UploaddedPost
            {
                Image = arr,
                Content = text,
                TagedUsersIds = tags
            });
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<Comment>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task<Post> PublishPost(string text, byte[] arr, string[] tags)
        {
            var response = await httpClient.PostAsJsonAsync($"PublishPost", new UploaddedPost
            {
                Image = arr,
                Content = text,
                TagedUsersIds = tags
            });
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<Post>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task Block(string userID)
        {
            var response = await httpClient.GetAsync($"BlockUser/{userID}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<IEnumerable<UserDetails>> GetFollowed()
        {
            var response = await httpClient.GetAsync("GetFollowedUsers");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<IEnumerable<UserDetails>>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }
    }
}
