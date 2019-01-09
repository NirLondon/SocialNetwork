using Identity.Common.Models;

namespace Identity.Server.Models
{
    public class EditDetailsModel
    {
        public string Token { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}