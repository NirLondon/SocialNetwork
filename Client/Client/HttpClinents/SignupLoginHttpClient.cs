using Client.DataProviders;
using Client.Enum;
using Client.Exeptions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class SignupLoginHttpClient : HttpHelper, ISignupLoginDataProvider
    {
        public SignupLoginHttpClient() : base("http://localhost:63172/api/SignupLogin/") { }
        //http://SocialNetwork.Authentication.com/api/SignupLogin/
        //http://localhost:63172/

        public Task Signup(string username, string password)
        {
            return SignupOrLogin($"Signup/{username}/{password}");
        }

        public Task Login(string username, string password)
        {
            return SignupOrLogin($"Login/{username}/{password}");
        }

        private async Task SignupOrLogin(string url)
        {
            var response = await httpClient.GetAsync(url);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var eror = response.Content.ReadAsAsync<ErrorEnum>().Result;
                    switch (eror)
                    {
                        case ErrorEnum.EverythingIsGood:
                            SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                            break;
                        case ErrorEnum.WrongUsernameOrPassword:
                            throw new WrongUsernameOrPasswordException();
                        case ErrorEnum.UsernameAlreadyExist:
                            throw new UsernameAlreadyExistException();
                        case ErrorEnum.UserIsBlocked:
                            throw new UserIsBlockedException();
                    }
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task LoginWithFacebook(string facebookToken)
        {
            var response = await httpClient.PostAsJsonAsync($"LoginWithFacebook", facebookToken);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var eror = response.Content.ReadAsAsync<ErrorEnum>().Result;
                    switch (eror)
                    {
                        case ErrorEnum.EverythingIsGood:
                            SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                            break;
                        case ErrorEnum.WrongUsernameOrPassword:
                            throw new WrongUsernameOrPasswordException();
                        case ErrorEnum.UsernameAlreadyExist:
                            throw new UsernameAlreadyExistException();
                        case ErrorEnum.UserIsBlocked:
                            throw new UserIsBlockedException();
                    }
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        public async Task SwitchToFacebookUser(string username, string password)
        {
            var response = await httpClient.GetAsync($"SwitchToFacebookUser/{username}/{password}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var eror = response.Content.ReadAsAsync<ErrorEnum>().Result;
                    switch (eror)
                    {
                        case ErrorEnum.EverythingIsGood:
                            SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                            break;
                        case ErrorEnum.WrongUsernameOrPassword:
                            throw new WrongUsernameOrPasswordException();
                        case ErrorEnum.UsernameAlreadyExist:
                            throw new UsernameAlreadyExistException();
                        case ErrorEnum.UserIsBlocked:
                            throw new UserIsBlockedException();
                    }
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }
    }
}
