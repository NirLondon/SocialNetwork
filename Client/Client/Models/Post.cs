using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.Models
{
    public class Post
    {
        public string Publisher { get; set; }
        public string PublisherID { get; set; }
        public string IMGURL { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public int Likes { get; set; }
        public bool DidLiked { get; set; }
        public string ID { get; set; }

        public ObservableCollection<Comment> Comments { get; set; } = new ObservableCollection<Comment>();
    }
}
