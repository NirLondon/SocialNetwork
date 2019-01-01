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
        // GET api/<controller>
        public void Signup(string username, string password)
        {
            UserModel user = new UserModel()
            {
                UserID = username,
                Password = password,
                State = Common.Enums.UserStateEnum.Open
            };

        }


        public void SignupWithFacebook(string facebbokToken)
        {

        }


        public void LoginWithFacebook(string facebookToken)
        {

        }
    }
}