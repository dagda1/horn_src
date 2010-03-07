using System;

namespace Horn.Core
{
    [global::System.Serializable]
    public class ProcessFailedException : Exception
    {
        protected ProcessFailedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public ProcessFailedException() { }

        public ProcessFailedException(int exitCode) : base("Process failed with Exit Code: " + exitCode) {}

        public ProcessFailedException(string message) : base(message) { }

        public ProcessFailedException(string message, Exception inner) : base(message, inner) { }
    }
}