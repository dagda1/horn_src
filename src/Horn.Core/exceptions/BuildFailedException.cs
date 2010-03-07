using System;

namespace Horn.Core
{
    public class BuildFailedException : Exception
    {
        protected BuildFailedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public BuildFailedException() { }

        public BuildFailedException(string message) : base(message) { }

        public BuildFailedException(string message, Exception inner) : base(message, inner) { }                
    }
}