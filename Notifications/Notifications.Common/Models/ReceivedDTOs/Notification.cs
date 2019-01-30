using System.Collections.Generic;

namespace Notifications.Common.Models.ReceivedDTOs
{
    public class Notification
    {
        public string Type { get; set; }
        public IDictionary<string, object> Data { get; set; }
        public string[] ReceiversIds { get; set; }
    }
}
