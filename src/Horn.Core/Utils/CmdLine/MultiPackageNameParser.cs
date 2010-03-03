using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Horn.Core.Utils.CmdLine
{
    public class MultiPackageNameParser
    {
        public PackageArgs[] Parse(IList<string> names)
        {
            return names
                .Select(name => Parse(name))
                .ToArray();
        }

        private static readonly Regex NameRegex = new Regex("^(?<PackageName>[^@#]+)(?:@(?<Version>[^#]+)){0,1}(?:#(?<Mode>.+)){0,1}");

        public PackageArgs Parse(string name)
        {
            if (!NameRegex.IsMatch(name))
            {
                return null;
            }

            Match match = NameRegex.Match(name);
            string packageName = match.Groups["PackageName"].Value;
            string version = match.Groups["Version"].Value;
            string mode = match.Groups["Mode"].Value;

            return new PackageArgs()
            {
                PackageName = EmptyToNull(packageName),
                Version = EmptyToNull(version),
                Mode = EmptyToNull(mode),
            };
        }

        private static string EmptyToNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}