using Client.DataProviders;
using Client.Enum;
using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class EditDetailsHttpClient : HttpHelper, IEditDetailsDataProvider
    {
        public EditDetailsHttpClient() : base("http://SocialNetwork.Social.com/Social/") { }

        public async Task<UserDetails> GetUserDetails()
        {
            UserDetails details = null;
            var response = await httpClient.GetAsync($"api/users/details/{CURRENTTOKEN}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<(string token, UserDetails)>();
                CURRENTTOKEN = result.token;
                details = result.Item2;
            }
            return details;
        }

        public async Task UpdateUserDetails(UserDetails userDetails)
        {
            var response = await httpClient.PutAsJsonAsync("api/users/editdetails",
                new
                {
                    Token = CURRENTTOKEN,
                    UserDetails = userDetails
                });

            if (response.IsSuccessStatusCode)
            {
                CURRENTTOKEN = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
