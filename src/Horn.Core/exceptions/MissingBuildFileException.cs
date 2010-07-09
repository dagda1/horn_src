using System;
using System.IO;

namespace Horn.Core
{
    [global::System.Serializable]
    public class MissingBuildFileException : Exception
    {
        public static string ErrorMessage(DirectoryInfo buildFolder)
        {
            return string.Format("No build file component {0} at path {1}.", buildFolder.Name, buildFolder.FullName);   
        }

        protected MissingBuildFileException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public MissingBuildFileException() { }

        public MissingBuildFileException(DirectoryInfo buildFolder) : base(ErrorMessage(buildFolder))
        {
        }

        public MissingBuildFileException(string message) : base(message) { }

        public MissingBuildFileException(DirectoryInfo buildFolder, Exception inner) : base(ErrorMessage(buildFolder), inner)
        {
        }

        public MissingBuildFileException(string message, Exception inner) : base(message, inner) { }
    }
}