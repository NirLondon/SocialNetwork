using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Common.Exceptions
{

    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string userID, string message = null) : base($"A user with {userID} UserID already exists") { }
        public UserAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected UserAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
