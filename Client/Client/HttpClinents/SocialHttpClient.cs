using Client.Models;
using Client.DataProviders;
using Client.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Net;
using Client.Exeptions;
using Social.Common.Models.UploadedDTOs;
using Social.Common.Models.ReturnedDTOs;

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

        public async Task UnFollow(string userID)
        {
            var response = await httpClient.GetAsync($"UnFollow/{userID}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<List<ReturnedPost>> GetPosts()
        {
            var response = await httpClient.GetAsync($"GetPosts/{SkipPostAmount}/{GetPostIndex}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return await response.Content.ReadAsAsync<List<ReturnedPost>>();
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task LikePost(Guid postID)
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

        public async Task DisLikePost(Guid postID)
        {
            var response = await httpClient.GetAsync($"UnLike/{postID}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<RetunredComment> Comment(string text, byte[] arr, string[] tags)
        {
            var response = await httpClient.PostAsJsonAsync($"PublishComment", new UploadedPost
            {
                Image = arr,
                Content = text,
                TagedUsersIds = tags
            });
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<RetunredComment>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task<ReturnedPost> Post(UploadedPost post)
        {
            var response = await httpClient.PostAsJsonAsync($"Post", post);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<ReturnedPost>().Result;
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

        public async Task UnBlock(string userID)
        {
            var response = await httpClient.GetAsync($"UnBlockUser/{userID}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task<List<UserDetails>> GetFollowed()
        {
            var response = await httpClient.GetAsync("Followed");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<List<UserDetails>>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task<List<UserDetails>> GetFollowers()
        {
            var response = await httpClient.GetAsync("Followers");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<List<UserDetails>>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task<List<UserDetails>> GetBlocked()
        {
            var response = await httpClient.GetAsync("Blocked");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<List<UserDetails>>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task<List<RetunredComment>> GetComments(Guid postID)
        {
            var response = await httpClient.GetAsync($"GetComments/{postID}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return response.Content.ReadAsAsync<List<RetunredComment>>().Result;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }
    }
}
