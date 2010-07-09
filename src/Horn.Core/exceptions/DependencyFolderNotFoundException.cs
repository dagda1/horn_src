using System;
using Horn.Core.PackageStructure;

namespace Horn.Core
{
    [global::System.Serializable]
    public class DependencyFolderNotFoundException : Exception
    {
        protected DependencyFolderNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public DependencyFolderNotFoundException() { }

        public DependencyFolderNotFoundException(string message) : base(message) { }

        public DependencyFolderNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}