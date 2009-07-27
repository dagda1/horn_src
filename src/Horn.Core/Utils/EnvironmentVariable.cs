using System;
using System.IO;

namespace Horn.Core.Utils
{
    public class EnvironmentVariable : IEnvironmentVariable
    {
        public string GetDirectoryFor(string fileName)
        {
            foreach (string item in Environment.GetEnvironmentVariable("path").Split(';'))
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                if (File.Exists(Path.Combine(item, "ruby.exe")))
                {
                    return item;
                }
            }
            return String.Empty;
        }
    }
}