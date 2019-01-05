using System;

namespace Identity.Common.Models
{
    public class UserDetails
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }
        public string Company { get; set; }
    }
}
