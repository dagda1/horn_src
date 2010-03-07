using System;

namespace Horn.Core
{
    public class NullTreeException : Exception
    {
        private const string ErrorMessage = "No package tree has been found";

        protected NullTreeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public NullTreeException() { }

        public NullTreeException(string message) : base(ErrorMessage) { }

        public NullTreeException(string message, Exception inner) : base(ErrorMessage, inner) { } 
    }
}