using System;

namespace Social.Common.Models.ReturnedDTOs
{
    public class ReturnedPost
    {
        public Guid PostId { get; set; }
        public UserMention Poster { get; set; }
        public string Content { get; set; }
        public DateTime UploadingTime { get; set; }
        public string ImageURL { get; set; }
        public bool IsLiked { get; set; }
        public UserMention[] Likes { get; set; }
        public UserMention[] Tags { get; set; }
    }
}