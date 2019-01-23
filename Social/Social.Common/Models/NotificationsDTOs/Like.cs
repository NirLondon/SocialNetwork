using System;

namespace Social.Common.Models.NotificationsDTOs
{
    public abstract class Like
    {
        public string AddresseeId { get; set; }
        public DateTime OccurrenceTime { get; set; }
        public string LikerId { get; set; }
        public object LikedItemId { get; set; }
    }
}
