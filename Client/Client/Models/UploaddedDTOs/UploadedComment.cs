using System;

namespace Client.Models.UploadedDTOs
{
    public class UploadedComment
    {
        public string Content { get; set; }
        public string[] TagedUsersIds { get; set; }
        public Guid PostId { get; set; }
        public byte[] Image { get; set; }
    }
}
