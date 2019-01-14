using System;

namespace Social.Common.Models
{
    public class Comment
    {
        public string Publisher { get; set; }
        public string IMGURL { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int Likes { get; set; }
        public bool DidLiked { get; set; }
        public int CommentId { get; set; }
        public int CommentedId { get; set; }
        public CommentOn CommentedOn { get; set; }

        public enum CommentOn
        {
            Post = 0,
            Comment = 1
        }
    }
}
