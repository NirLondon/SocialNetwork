using Client.Enum;
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

        public async Task<Tuple<string, ErrorEnum>> Signup(string username, string password)
        {
            string token = null;                           // string and eror 
            ErrorEnum eror = ErrorEnum.EverythingIsGood;   // enum to return
            var result = await httpClient.GetAsync($"Signup/{username}/{password}");

            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsAsync<Tuple<string, ErrorEnum>>().Result;
                eror = response.Item2;
                token = response.Item1;
            }
            Tuple<string, ErrorEnum> tuple = new Tuple<string, ErrorEnum>(token, eror);
            return tuple;
        }

        public async Task<Tuple<string, ErrorEnum>> Login(string username, string password)
        {
            string token = null;
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            var result = await httpClient.GetAsync($"Login/{username}/{password}");
            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsAsync<Tuple<string, ErrorEnum>>().Result;
                eror = response.Item2;
                token = response.Item1;
            }
            else
            {
                eror = ErrorEnum.ConectionFailed;
            }
            Tuple<string, ErrorEnum> tuple = new Tuple<string, ErrorEnum>(token, eror);
            return tuple;
        }
    }
}
