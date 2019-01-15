using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Exeptions
{

    [Serializable]
    public class TokenExpiredExeption : Exception
    {
        public TokenExpiredExeption():this("The token has expired. ") { }
        public TokenExpiredExeption(string message) : base(message) { }
        public TokenExpiredExeption(string message, Exception inner) : base(message, inner) { }
        protected TokenExpiredExeption(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
