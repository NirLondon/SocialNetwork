﻿using System;

namespace Authentication.Common.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
