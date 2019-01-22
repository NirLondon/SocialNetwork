﻿using System;

namespace Client.Models.ReturnedDTOs
{
    public class RetunredComment
    {
        public Guid CommentId { get; set; }
        public UserMention Commenter { get; set; }
        public string Content { get; set; }
        public DateTime UploadingTime { get; set; }
        public string ImageURL { get; set; }
        public bool IsLiked { get; set; }    
        public UserMention[] Likes { get; set; }
        public UserMention[] Tags { get; set; }
    }
}
