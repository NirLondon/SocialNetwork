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

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}