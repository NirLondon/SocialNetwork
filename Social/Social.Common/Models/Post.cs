using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Social.Common.Models
{
    public class Post
    {
        public string Publisher { get; set; }
        public string IMGURL { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public int Likes { get; set; }
        public bool DidLiked { get; set; }
        public int ID { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
