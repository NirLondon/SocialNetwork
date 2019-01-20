using Authentication.Common.BL;
using System;
using System.Net.Http;

namespace Authentication.BL
{
    public class Notifier : INotifier
    {
        private readonly HttpClient httpClient;

        public Notifier()
        {
            httpClient = new HttpClient();
        }

        public void NotifyToIdentityService(string userId, string token)
        {
            using (httpClient)
            {
                SetToken(token);
                var response = httpClient
                    .PostAsJsonAsync($"http://localhost:63276/api/Users/Add", userId) //http://SocialNetwork.Identity.com
                    .Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }
        }

        public void NotifyToSocailService(string userId, string token)
        {
            using (httpClient)
            {
                SetToken(token);
                var response = httpClient
                    .PostAsJsonAsync($"http://localhost:63377/api/Social/Users/Add", userId)
                    .Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }
        }

        private void SetToken(string token)
        {
            httpClient.DefaultRequestHeaders.Remove("Token");
            httpClient.DefaultRequestHeaders.Add("Token", token);
        }

        ~Notifier()
        {
            httpClient.Dispose();
        }
    }
}
