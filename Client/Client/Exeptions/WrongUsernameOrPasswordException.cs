using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Exeptions
{

    [Serializable]
    public class WrongUsernameOrPasswordException : Exception
    {
        public WrongUsernameOrPasswordException() : base("Wrong username or password. ") { }
        public WrongUsernameOrPasswordException(string message) : base(message) { }
        public WrongUsernameOrPasswordException(string message, Exception inner) : base(message, inner) { }
        protected WrongUsernameOrPasswordException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
