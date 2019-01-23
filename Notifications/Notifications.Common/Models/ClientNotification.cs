using System.Collections.Generic;

namespace Notifications.Common.Models
{
    public class ClientNotification
    {
        public string Type { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}
