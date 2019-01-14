using System;

namespace Identity.Common.Models
{
    public class UserDetails
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Job { get; set; }
    }
}