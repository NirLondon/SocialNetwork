using Authentication.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string AssignedUser { get; set; }
        public TokenStateEnum State { get; set; }
    }
}
