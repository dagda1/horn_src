using System;

namespace Horn.Core
{
    public class CannotDeleteTempHornDirectoryException : Exception
    {
        protected CannotDeleteTempHornDirectoryException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public CannotDeleteTempHornDirectoryException() { }

        public CannotDeleteTempHornDirectoryException(string message) : base(message) { }

        public CannotDeleteTempHornDirectoryException(string message, Exception inner) : base(message, inner) { }         
    }
}