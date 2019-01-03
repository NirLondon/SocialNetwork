using Authentication.Common.BL;
using Authentication.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<TokenModel> Signup(string username, string password)
        {
            UserModel user = GenerateUser(username, password);

            await repository.Signup(user)
                /*.Wait()*/;

            TokenModel TM = GenerateToken(username);
            repository.SaveToken(TM);
            return TM;
        }


        private TokenModel GenerateToken(string username)
        {
            TokenModel TM = new TokenModel()
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                AssignedUser = username,
                State = Common.Enums.TokenStateModel.Valid
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