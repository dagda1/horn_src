using System;

namespace Horn.Core.exceptions
{
    public class EnvironmentVariableNotFoundException : Exception
    {
        protected EnvironmentVariableNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public EnvironmentVariableNotFoundException() { }

        public EnvironmentVariableNotFoundException(string message) : base(message) { }

        public EnvironmentVariableNotFoundException(string message, Exception inner) : base(message, inner) { }                
    }
}
