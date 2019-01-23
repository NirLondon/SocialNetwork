using System;
using System.Collections.Generic;

namespace Notifications.Common.Models
{
    [Serializable]
    public class Notification
    {
        private string _type;

        public string[] ReceiversIds { get; set; }
        public string Type
        {
            get => _type;
            set => _type = value.Trim().ToLower(); 
        }
        public IDictionary<string, object> Data { get; set; }
    }
}
