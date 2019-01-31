using Newtonsoft.Json;
using Social.Common.BL;
using System.Net.Http;
using System.Threading.Tasks;

namespace Social.BL
{
    public class Authenticator : IAuthentiacator
    {
        public async Task<(string Token, string UserId)> Authenticate(string token)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:63172/api/Tokens/Validate/{token}");
                //var response = await httpClient.GetAsync($"http://SocialNetwork.Authentication.com/api/Tokens/Validate/{token}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<(string Token, string UserId)>(json);

                    return result;
                }

                return (null, null);
            }
        }
    }
}
