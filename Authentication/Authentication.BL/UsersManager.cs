using Authentication.Common;
using Authentication.Common.BL;
using Authentication.Common.DAL;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
using Authentication.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        public SignupLoginResult LoginWithFacebook(string facebookToken)
        {
            string token = null;
            SignupLoginResult eror = SignupLoginResult.EverythingIsGood;
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://graph.facebook.com/me?access_token=" + facebookToken).Result;
            if (result.IsSuccessStatusCode)
            {
                var facebookUser = result.Content.ReadAsStringAsync().Result;
                var parsedUser = JObject.Parse(facebookUser).ToObject<dynamic>();
                UserModel user = GenerateUser("_" + parsedUser.id.Value, "");
                eror = _usersManager.LoginWithFacebook(user);
                if (eror == SignupLoginResult.EverythingIsGood)
                {
                    TokenModel TM = GenerateToken(user.UserID);
                    _usersManager.SaveToken(TM);
                    token = TM.Token;
                }
            }
            else
                eror = SignupLoginResult.WrongUsernameOrPassword;

            Tuple<string, SignupLoginResult> tuple = new Tuple<string, SignupLoginResult>(token, eror);
            return tuple;
        }

        public SignupLoginResult ResetPassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void SaveToken(TokenModel token)
        {
            _repository.SaveToken(token);
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
            var eror = _repository.SwitchToFacebookUser(username, passwrod);
            return eror;
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
