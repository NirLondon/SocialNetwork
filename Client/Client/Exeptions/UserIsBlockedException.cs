using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Exeptions
{

    [Serializable]
    public class UserIsBlockedException : Exception
    {
        public UserIsBlockedException() : base("User is blocked. ") { }
        public UserIsBlockedException(string message) : base(message) { }
        public UserIsBlockedException(string message, Exception inner) : base(message, inner) { }
        protected UserIsBlockedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
