using Client.Common;
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


        public async Task<Tuple<ErrorEnum, Post>> PublishPost(string text, byte[] arr)
        {
            var error = ErrorEnum.ConectionFailed;
            Post post = null;
            
            var response = await httpClient.PostAsJsonAsync($"PublishPost", (CURRENTTOKEN, text, arr));

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<(ErrorEnum, Post, string)>().Result;
                error = result.Item1;
                post = result.Item2;
                CURRENTTOKEN = result.Item3; 
            }
            var tuple = new Tuple<ErrorEnum, Post>(error, post);
            return tuple;
        }
    }
}
