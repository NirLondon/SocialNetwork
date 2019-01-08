using System.Collections.Generic;

namespace Identity.Server.Models
{
    public class EditDetailsModel
    {
        public string Token { get; set; }
        public Dictionary<string, object> EditedDetails { get; set; }
    }
}