using Client.DataProviders;
using Client.Enum;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class SignupLoginHttpClient : HttpHelper, ISignupLoginDataProvider
    {
        public SignupLoginHttpClient() : base("http://localhost:63172/api/SignupLogin/") { }

        public Task<ErrorEnum> Signup(string username, string password)
        {
            return SignupOrLogin($"Signup/{username}/{password}");
        }

        public Task<ErrorEnum> Login(string username, string password)
        {
            return SignupOrLogin($"Login/{username}/{password}");
        }

        private async Task<ErrorEnum> SignupOrLogin(string url)
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<Tuple<string, ErrorEnum>>().Result;
                CURRENTTOKEN = result.Item1;
                return result.Item2;
            }
            return ErrorEnum.ConectionFailed;
        }

        public async Task<ErrorEnum> LoginWithFacebook(string facebookToken)
        {
            var response = await httpClient.PostAsJsonAsync($"LoginWithFacebook", facebookToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Tuple<string, ErrorEnum>>();
                CURRENTTOKEN = result.Item1;
                return result.Item2;
            }
            return ErrorEnum.ConectionFailed;
        }

        public async Task<ErrorEnum> SwitchToFacebookUser(string username, string password)
        {
            var result = await httpClient.GetAsync($"SwitchToFacebookUser/{username}/{password}");

            if (result.IsSuccessStatusCode)
                return await result.Content.ReadAsAsync<ErrorEnum>();
            return ErrorEnum.ConectionFailed;
        }
    }
}
