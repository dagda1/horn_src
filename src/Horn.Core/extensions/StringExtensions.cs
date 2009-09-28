using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Horn.Core.extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Quotes a string to make it suitable for the command line.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QuotePath(this String str)
        {
            if(string.IsNullOrEmpty(str))
                return str;

            if (str.StartsWith("\"") && str.EndsWith("\""))
                return str;
            return String.Format("\"{0}\"", str);
        }

        public static string RemoveDebugFolderParts(this string part)
        {            
            var ret = part.Replace("bin\\x86\\Debug\\", string.Empty).Replace("bin\\Debug", string.Empty);

            return ret;
        }

        public static bool IsNumeric(this string text)
        {
            return Regex.IsMatch(text, @"^\d+$");
        }
    }
}