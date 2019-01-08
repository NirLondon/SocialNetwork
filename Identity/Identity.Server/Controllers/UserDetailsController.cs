using Identity.Common.DAL;
using Identity.Server.Models;
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
        public async Task EditUserDetails([FromBody] EditDetailsModel model)
        {
            var userId = await GetUserIdFromToken(model.Token);

            if (userId != null)
                await repository.EditUser(userId, model.EditedDetails);
        }


        [HttpGet]
        [Route("api/users/details/{token}")]
        public async Task<string> GetUserDetails(string token)
        {
            var userId = await GetUserIdFromToken(token);

            if (userId != null)
                return await repository.GetDetailsJsonAsync(userId);
            return string.Empty;
        }

        private async Task<string> GetUserIdFromToken(string token)
        {
            var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:63172/") };

            using (httpClient)
            {
                var response = await httpClient.GetAsync($"api/ValidateToken/Validate/{token}");

                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<string>().Result;
            }

            throw new Exception("The authentication service is not available. ");
        }
    }
}
