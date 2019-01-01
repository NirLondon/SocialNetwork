using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class SignupLoginHttpClient
    {
        private HttpClient httpClient { get; set; }
        private string api { get; set; }


        public SignupLoginHttpClient()
        {
            httpClient = new HttpClient();
            api = "http://localhost:63172/api/SignupLogin/";
            httpClient.BaseAddress = new Uri(api);
        }



        public async Task<bool> Signup(string username, string password)
        {
            bool flag = false;
            var result = await httpClient.GetAsync($"Signup/{username}/{password}");

            if (result.IsSuccessStatusCode)
            {
                flag = true;
            }
            return flag;
        }

        public async Task<bool> Login(string username, string password)
        {
            bool flag = false;
            var result = await httpClient.GetAsync($"Login/{username}/{password}");
            if (result.IsSuccessStatusCode)
            {
                flag = result.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                //manage error
            }

            return flag;
        }
    }
}
