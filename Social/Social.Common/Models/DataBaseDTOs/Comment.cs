using System;

namespace Social.Common.Models.DataBaseDTOs
{
    public class DataBaseComment
    {
        public string Content { get; set; }
        public string[] TagedUsersIds { get; set; }
        public Guid PostId { get; set; }
        public string ImagURL { get; set; }
    }
}
