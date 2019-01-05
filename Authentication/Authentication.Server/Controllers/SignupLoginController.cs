using Authentication.Common.BL;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.Server.Controllers
{
    public class SignupLoginController : ApiController
    {
        ISignupLogin repository;
        const string api = "api/SignupLogin/";

        public SignupLoginController(ISignupLogin repositoryManager)
        {
            repository = repositoryManager;
        }

        [HttpGet]
        [Route(api + "Signup/{username}/{password}")]
        public Tuple<string, ErrorEnum> Signup(string username, string password)
        {
            Tuple<string, ErrorEnum> response;
            string token = null;
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            UserModel user = GenerateUser(username, password);
            if (ValidateValues(username, password))
            {
                try
                {
                    repository.Signup(user);
                    TokenModel TM = GenerateToken(username);
                    token = TM.Token;
                    repository.SaveToken(TM);
                }
                catch (Exception e)
                {
                    eror = ManageError(e);
                }
            }
            else
                eror = ErrorEnum.WrongUsernameOrPassword;
            response = new Tuple<string, ErrorEnum>(token, eror);
            return response; ;
        }

        [HttpGet]
        [Route(api + "Login/{username}/{password}")]
        public Tuple<string, ErrorEnum> Login(string username, string password)
        {
            string token = null;
            ErrorEnum eror = ErrorEnum.WrongUsernameOrPassword;
            if (password != null || password != "")
            {
                UserModel user = GenerateUser(username, password);
                eror = repository.Login(user);
                if (eror == ErrorEnum.EverythingIsGood)
                {
                    TokenModel TM = GenerateToken(username);
                    repository.SaveToken(TM);
                    token = TM.Token;
                }
            }
            Tuple<string, ErrorEnum> tuple = new Tuple<string, ErrorEnum>(token, eror);
            return tuple;
        }

        [HttpPost]
        [Route(api + "LoginWithFacebook")]
        public Tuple<string, ErrorEnum> LoginWithFacebook([FromBody]string facebookToken)
        {
            string token = null;
            ErrorEnum eror = ErrorEnum.EverythingIsGood;
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://graph.facebook.com/me?access_token=" + facebookToken).Result;
            if (result.IsSuccessStatusCode)
            {
                var facebookUser = result.Content.ReadAsStringAsync().Result;
                var parsedUser = JObject.Parse(facebookUser).ToObject<dynamic>();
                UserModel user = GenerateUser("_" + parsedUser.id.Value, "");
                eror = repository.LoginWithFacebook(user);
                if (eror == ErrorEnum.EverythingIsGood)
                {
                    TokenModel TM = GenerateToken(user.UserID);
                    repository.SaveToken(TM);
                    token = TM.Token;
                }
            }
            else
                eror = ErrorEnum.WrongUsernameOrPassword;

            Tuple<string, ErrorEnum> tuple = new Tuple<string, ErrorEnum>(token, eror);
            return tuple;
        }

        [HttpGet]
        [Route(api + "SwitchToFacebookUser/{username}/{password}")]
        public ErrorEnum SwitchToFacebookUser(string username, string password)
        {
            var eror = repository.SwitchToFacebookUser(username, password);
            return eror;
        }

        [HttpGet]
        [Route(api + "ResetPassword/{username}/{oldPassword}/{newPassword}")]
        public ErrorEnum resetPassword(string username, string oldPassword, string newPassword)
        {
            var eror = repository.ResetPassword(username, oldPassword, newPassword);
            return eror;
        }


        private TokenModel GenerateToken(string username)
        {
            TokenModel TM = new TokenModel()
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                AssignedUser = username,
                State = Common.Enums.TokenStateEnum.Valid
            };
            return TM;
        }

        private UserModel GenerateUser(string username, string password)
        {
            UserModel user = new UserModel()
            {
                UserID = username,
                Password = password,
                State = Common.Enums.UserStateEnum.Open
            };
            return user;
        }

        private bool ValidateValues(string username, string password)
        {
            bool flag = false;
            if (username.Length > 0 && username[0] != '_')
            {
                if (password.Length > 0)
                    flag = true;
            }
            return flag;
        }

        private ErrorEnum ManageError(Exception e)
        {
            ErrorEnum er = ErrorEnum.EverythingIsGood;
            if (e is UserAlreadyExistsException)
            {
                er = ErrorEnum.UsernameAlreadyExist;
            }
            else
            {
                er = ErrorEnum.ConectionFailed;
            }
            return er;
        }
    }
}