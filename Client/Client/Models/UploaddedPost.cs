using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class UploaddedPost
    {
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public string[] TagedUsersIds { get; set; }
    }
}
