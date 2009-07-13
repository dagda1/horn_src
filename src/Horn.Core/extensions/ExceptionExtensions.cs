using System;

namespace Horn.Core.extensions
{
    public static class ExceptionExtensions
    {
        public static string UnwrapException(this Exception exception)
        {
            var result = string.Format("{0}\n", exception.Message);

            var innerException = exception.InnerException;

            while (innerException != null)
            {
                result += string.Format("{0}\n", innerException.Message);

                innerException = innerException.InnerException;
            }

            return result;
        }
    }
}