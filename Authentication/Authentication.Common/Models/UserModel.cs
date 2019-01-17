using Authentication.Common.Enums;

namespace Authentication.Common.Models
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public UserState State { get; set; }
    }
}
