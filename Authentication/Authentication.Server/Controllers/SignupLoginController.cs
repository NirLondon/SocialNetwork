using Authentication.Common.BL;
using Authentication.Common.Enums;
using Authentication.Common.Exceptions;
using Authentication.Common.Models;
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
            try
            {
                repository.Signup(user);
                TokenModel TM = GenerateToken(username);
                token = TM.Token;
                repository.SaveToken(TM);
            }
            catch (Exception e)
            {
                if (e is UserAlreadyExistsException)
                {
                    eror = ErrorEnum.UsernameAlreadyExist;
                }
                else
                {
                    eror = ErrorEnum.ConectionFailed;
                }
            }
            response = new Tuple<string, ErrorEnum>(token, eror);
            return response; ;
        }

        [HttpGet]
        [Route(api + "Login/{username}/{password}")]
        public Tuple<string, ErrorEnum> Login(string username, string password)
        {
            string token = null;
            ErrorEnum eror = ErrorEnum.WrongUsernameOrPassword;
            UserModel user = GenerateUser(username, password);
            bool success = repository.Login(user);
            if (success)
            {
                eror = ErrorEnum.EverythingIsGood;
                TokenModel TM = GenerateToken(username);
                repository.SaveToken(TM);
                token = TM.Token;
            }
            Tuple<string, ErrorEnum> tuple = new Tuple<string, ErrorEnum>(token, eror);
            return tuple;
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
    }
}