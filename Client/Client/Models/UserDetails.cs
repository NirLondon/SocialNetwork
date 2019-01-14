using System;

namespace Client.Models
{
    public class UserDetails
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Job { get; set; }
        public bool IsBlocked { get; set; }
    }
}