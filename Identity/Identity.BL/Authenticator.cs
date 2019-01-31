using Newtonsoft.Json;
using Identity.Common.BL;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.BL
{
    public class Authenticator : IAuthentiacator
    {
        public async Task<(string Token, string UserId)> Authenticate(string token)
        {
            using (var httpClient = new HttpClient())
            {
                //var response = await httpClient.GetAsync($"http://SocialNetwork.Authentication.com/api/Tokens/Validate/{token}");
                var response = await httpClient.GetAsync($"http://localhost:63172/api/Tokens/Validate/{token}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<(string Tken, string UserId)>(json);

                    return result;
                }

                return (null, null);
            }
        }
    }
}
