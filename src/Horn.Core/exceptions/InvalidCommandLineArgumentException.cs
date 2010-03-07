using System;

namespace Horn.Core
{
    [global::System.Serializable]
    public class InvalidCommandLineArgumentException : Exception
    {
        protected InvalidCommandLineArgumentException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public InvalidCommandLineArgumentException() { }

        public InvalidCommandLineArgumentException(string message) : base(message) { }

        public InvalidCommandLineArgumentException(string message, Exception inner) : base(message, inner) { }
    }
}