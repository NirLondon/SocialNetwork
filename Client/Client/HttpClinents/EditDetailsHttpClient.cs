using Client.DataProviders;
using Client.Enum;
using Client.Exeptions;
using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class EditDetailsHttpClient : HttpHelper, IEditDetailsDataProvider
    {
        public EditDetailsHttpClient() : base("http://localhost:63276/api/users/") { }
        //http://localhost:63276/
        //http://SocialNetwork.Social.com/
        public async Task<UserDetails> GetUserDetails()
        {
            var response = await httpClient.GetAsync($"api/users/GetUserDetails");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    return await response.Content.ReadAsAsync<UserDetails>();
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
            return null;
        }

        public async Task UpdateUserDetails(UserDetails userDetails)
        {
            var response = await httpClient.PutAsJsonAsync("api/UserDetails/EditDetails", userDetails);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsAsync<UserDetails>();
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }
    }
}
