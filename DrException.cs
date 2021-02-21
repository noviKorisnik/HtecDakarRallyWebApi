using System;
using System.Runtime.Serialization;

namespace HtecDakarRallyWebApi
{
    public class DrException : Exception
    {
        public DrException() : base() { }
        public DrException(string message) : base(message) { }
        public DrException(string message, Exception innerException) : base(message, innerException) { }
        protected DrException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}