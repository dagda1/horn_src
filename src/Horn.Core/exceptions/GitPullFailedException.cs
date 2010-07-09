using System;

namespace Horn.Core.exceptions
{
    public class GitPullFailedException : Exception
    {
        protected GitPullFailedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public GitPullFailedException() { }

        public GitPullFailedException(string message) : base(message) { }

        public GitPullFailedException(string message, Exception inner) : base(message, inner) { }          
    }
}