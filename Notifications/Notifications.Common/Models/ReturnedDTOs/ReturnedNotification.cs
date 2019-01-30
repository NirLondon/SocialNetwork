using System.Collections.Generic;

namespace Notifications.Common.Models.ReturnedDTOs
{
    public class ReturnedNotification
    {
        public string Type { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}
