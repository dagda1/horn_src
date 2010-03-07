using System;

namespace Horn.Core
{
    [Serializable]
    public class RemoteScmException : Exception
    {
        protected RemoteScmException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public RemoteScmException() { }

        public RemoteScmException(string message) : base(message) { }

        public RemoteScmException(string message, Exception inner) : base(message, inner) { }        
    }
}