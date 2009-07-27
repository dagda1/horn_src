using System;

namespace Horn.Core
{
    [Serializable]
    public class UnknownInstallPackageException : Exception
    {
        protected UnknownInstallPackageException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public UnknownInstallPackageException() { }

        public UnknownInstallPackageException(string message) : base(message) { }

        public UnknownInstallPackageException(string message, Exception inner) : base(message, inner) { }        
    }
}