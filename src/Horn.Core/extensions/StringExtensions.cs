using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Horn.Core.extensions
{
    public static class StringExtensions
    {
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