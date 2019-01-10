﻿using Client.DataProviders;
using Client.Enum;
using Identity.Common.Models;
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
        public EditDetailsHttpClient() : base("http://localhost:63276/") { }

        public async Task<UserDetails> GetUserDetails()
        {
            var response = await httpClient.GetAsync($"api/users/details/{CURRENTTOKEN}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<(string token, UserDetails)>();
                CURRENTTOKEN = result.token;
                return result.Item2;
            }

            throw new Exception();
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
                return;
            }
            throw new Exception();
        }
    }
}
