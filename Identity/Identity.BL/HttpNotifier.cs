using Identity.Common.BL;
using Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Identity.BL
{
    public class HttpNotifier : INotifier
    {
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

        public void NotifyToSocialService(UserToSocial user)
        {
            var response = httpClient
                //.PutAsJsonAsync("http://SocailNetwork.Social/api/Social/Users/Edit", user)
                .PutAsJsonAsync("http://localhost:63377/api/Social/Users/Edit", user)
                .Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception();
        }

        private readonly HttpClient httpClient;

        public HttpNotifier()
        {
            httpClient = new HttpClient();
        }

        ~HttpNotifier()
        {
            httpClient.Dispose();
        }
    }
}
