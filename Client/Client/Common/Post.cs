using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.Common
{
    public class Post
    {
        public string Publisher { get; set; }
        public string IMGURL { get; set; }
        public string Text { get; set; }
        public ObservableCollection<Comment> Comments { get; set; } = new ObservableCollection<Comment>();
    }
}
