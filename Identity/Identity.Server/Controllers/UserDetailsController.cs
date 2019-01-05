using Identity.Common.DAL;
using Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async void EditUserDetails(string token, [FromBody] Dictionary<string, object> userDetails)
        {
            if(await GetUserIdFromToken(token) != null)
            {
                repository.EditUser(userDetails);
            }
        }


        private async Task<string> GetUserIdFromToken(string token)
        {
            var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:63172/") };

            using (httpClient)
            {
                var response = await httpClient.PostAsJsonAsync("api/tokens/getuserid", token);

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<string>().Result;
                }
            }

            throw new Exception();
        }
    }
}
