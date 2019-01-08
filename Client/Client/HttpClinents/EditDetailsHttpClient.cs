using Client.DataProviders;
using Client.Enum;
using Newtonsoft.Json;
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

        public async Task<Tuple<ErrorEnum, IDictionary<string, object>>> GetUserDetails()
        {
            var response = await httpClient.GetAsync($"api/users/details/{CURRENTTOKEN}");

            if (response.IsSuccessStatusCode)
            {
                return new Tuple<ErrorEnum, IDictionary<string, object>>
                    (ErrorEnum.EverythingIsGood,
                    JsonConvert.DeserializeObject<Dictionary<string, object>>
                    (await response.Content.ReadAsStringAsync()));
            }

            return new Tuple<ErrorEnum, IDictionary<string, object>>(ErrorEnum.ConectionFailed, null);
        }

        public async Task UpdateUserDetails(IDictionary<string, object> userDetails)
        {
            var response = await httpClient.PostAsJsonAsync("api/users/editdetails",
                new
                {
                    Token = CURRENTTOKEN,
                    EditedDetails = userDetails
                });

            if (!response.IsSuccessStatusCode) ;
        }
    }
}
