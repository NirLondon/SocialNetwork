using Identity.Common.DAL;
using Identity.Common.Models;
using Identity.Server.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Identity.Server.Controllers
{
    public class UserDetailsController : ApiController
    {
        private readonly IIdentitiesRepository repository;

        public UserDetailsController(IIdentitiesRepository repository)
        {
            this.repository = repository;
        }

        [HttpPut]
        [Route("api/users/editdetails")]
        public async Task<string> EditUserDetails([FromBody] EditDetailsModel model)
        {
            var tokenUser = await GetUserIdAndTokenFromToken(model.Token);

            if (tokenUser.UserId != null)
            {
                await repository.EditUser(tokenUser.UserId, model.UserDetails);
                return tokenUser.Token;
            }
            return null;
        }
    
        [HttpGet]
        [Route("api/users/details/{token}")]
        public async Task<(string Token, UserDetails)> GetUserDetails(string token)
        {
            var tokenUser = await GetUserIdAndTokenFromToken(token);

            if (tokenUser.UserId != null)
                return (tokenUser.Token, await repository.GetUserDetailsAsync(tokenUser.UserId));
            return (null, null);
        }

        private async Task<(string Token, string UserId)> GetUserIdAndTokenFromToken(string token)
        {
            var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:63172/") };

            using (httpClient)
            {
                var response = await httpClient.GetAsync($"api/Tokens/Validate/{token}");

                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<(string, string)>().Result;

                throw new Exception("The authentication service is not available. ");
            }
        }
    }
}
