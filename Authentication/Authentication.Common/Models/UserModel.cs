using Authentication.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.Models
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public UserStateEnum State { get; set; }
    }
}
