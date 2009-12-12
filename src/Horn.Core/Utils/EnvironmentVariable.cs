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
				
				var path = item.Replace("\"", string.Empty);

                if (File.Exists(Path.Combine(path, fileName)))
                {
                    return path;
                }
            }
            return String.Empty;
        }
    }
}