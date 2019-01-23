using Newtonsoft.Json;
using Social.Common.BL;
using Social.Common.Models.NotificationsDTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Social.BL
{
    public class Notifier : INotifier
    {
        private readonly HttpClient httpClient;

        public Notifier()
        {
            httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:63304/") };
        }

        public async Task NotifyLike(Like like)
        {
            var response = await httpClient.PostAsync(
                "api/Notify/Like", 
                new StringContent(JsonConvert.SerializeObject(like)));

            switch (response.StatusCode)
            {
                default: break;
            }
        }

        ~Notifier()
        {
            httpClient.Dispose();
        }
    }
}
