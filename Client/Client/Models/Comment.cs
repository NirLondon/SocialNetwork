using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class Comment
    {
        public string Publisher { get; set; }
        public string PublisherID { get; set; }
        public string IMGURL { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public int Likes { get; set; }
        public bool DidLiked { get; set; }
        public int ID { get; set; }
    }
}