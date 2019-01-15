using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Exeptions
{

    [Serializable]
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException() :base("Username already exist. ") { }
        public UsernameAlreadyExistException(string message) : base(message) { }
        public UsernameAlreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected UsernameAlreadyExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
