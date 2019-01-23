using Authentication.Common.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Authentication.BL
{
    public class Notifier : INotifier
    {
        private readonly HttpClient httpClient;

        public string Token
        {
            get
            {
               return (httpClient.DefaultRequestHeaders.TryGetValues("Token", out IEnumerable<string> values)) ? 
                     values.FirstOrDefault() : null;
            }
            set
            {
                httpClient.DefaultRequestHeaders.Remove("Token");
                httpClient.DefaultRequestHeaders.Add("Token", value);
            }
        }

        public Notifier()
        {
            httpClient = new HttpClient();
        }

        public void NotifyToIdentityService(string userId)
        {
            var response = httpClient
                .PostAsJsonAsync($"http://localhost:63276/api/Users/Add", userId) //http://SocialNetwork.Identity.com
                .Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception();
        }

        public void NotifyToSocailService(string userId)
        {
            var response = httpClient
                .PostAsJsonAsync($"http://SocialNetwork.Social.com/api/Social/Users/Add", userId)
                .Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception();
        }

        ~Notifier()
        {
            httpClient.Dispose();
        }
    }
}
