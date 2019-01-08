using System;
using System.Net.Http;
using Authentication.Common.BL;
using Authentication.Common.DAL;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using Newtonsoft.Json.Linq;

namespace Authentication.BL
{
    public class UsersManager : IUsersManager
    {
        private readonly IUsersRepository _repository;

        public UsersManager(IUsersRepository repository)
        {
            _repository = repository;
        }

        public Tuple<string, SignupLoginResult> Login(string username, string password)
        {
            string token = null;
            SignupLoginResult eror = SignupLoginResult.WrongUsernameOrPassword;
            if (password != null || password != "")
            {
                UserModel user = new UserModel
                {
                    UserID = username,
                    Password = password,
                    State = UserState.Open
                };
                eror = _repository.Login(user);
                if (eror == SignupLoginResult.EverythingIsGood)
                {
                    TokenModel TM = new TokenModel
                    {
                        Token = Utils.GetNewToken(),
                        UserId = username,
                        CreationTime = DateTime.Now
                    };
                    _repository.SaveToken(TM);
                    token = TM.Token;
                }
            }
            return new Tuple<string, SignupLoginResult>(token, eror);
        }

        public Tuple<string, SignupLoginResult> LoginWithFacebook(string facebookToken)
        {
            string token = null;
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://graph.facebook.com/me?access_token=" + facebookToken).Result;
            if (result.IsSuccessStatusCode)
            {
                var facebookUser = result.Content.ReadAsStringAsync().Result;
                var parsedUser = JObject.Parse(facebookUser).ToObject<dynamic>();
                UserModel user = new UserModel
                {
                    UserID = '_' + parsedUser.id.Value,
                    Password = string.Empty,
                    State = UserState.Open
                };

                eror = _repository.LoginWithFacebook(user);
                if (eror == SignupLoginResult.EverythingIsGood)
                {
                    TokenModel TM = new TokenModel
                    {
                        UserId = user.UserID,
                        CreationTime = DateTime.Now,
                        Token = Utils.GetNewToken()
                    };
                    _repository.SaveToken(TM);
                    token = TM.Token;
                }
            }
            else eror = SignupLoginResult.WrongUsernameOrPassword;

            return new Tuple<string, SignupLoginResult>(token, eror);
        }

        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            return _repository.ResetPassword(username, oldPassword, newPassword);
        }

        public Tuple<string, SignupLoginResult> Signup(string username, string password)
        {
            string token = null;
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            if (Utils.IsValid(username, password))
            {
                UserModel user = new UserModel
                {
                    UserID = username,
                    Password = password,
                    State = UserState.Open
                };
                try
                {
                    _repository.Signup(user);
                    TokenModel TM = new TokenModel
                    {
                        Token = Utils.GetNewToken(),
                        UserId = username,
                        CreationTime = DateTime.Now
                    };
                    _repository.SaveToken(TM);
                    token = TM.Token;
                }
                catch (Exception e)
                {
                    eror = ManageError(e);
                }
            }
            else
                eror = SignupLoginResult.WrongUsernameOrPassword;
            return new Tuple<string, SignupLoginResult>(token, eror);
        }

        public SignupLoginResult SwitchToFacebookUser(string username, string passwrod)
        {
            return _repository.SwitchToFacebookUser(username, passwrod);
        }

        private SignupLoginResult ManageError(Exception exception)
        {
            SignupLoginResult er = SignupLoginResult.EverythingIsGood;
            switch (exception)
            {
                case UserAlreadyExistsException alreadyExists:
                    er = SignupLoginResult.UsernameAlreadyExist;
                    break;

                default:
                    er = SignupLoginResult.ConectionFailed;
                    break;
            }
            return er;
        }
    }
}
