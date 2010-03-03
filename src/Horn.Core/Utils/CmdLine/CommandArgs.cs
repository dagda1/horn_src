using System;
using System.Collections.Generic;

namespace Horn.Core.Utils.CmdLine
{
    public class CommandArgs : ICommandArgs
    {
        public const string IoCKey = "commandargs";

        public virtual PackageArgs[] Packages { get; private set; }

        public virtual bool RebuildOnly { get; private set; }

        public virtual bool Refresh { get; private set; }

        public virtual string OutputPath { get; private set; }

        public CommandArgs(IDictionary<string, IList<string>> switches)
        {
            if (switches.ContainsKey("install"))
            {
                PackageArgs single = new PackageArgs()
                {
                    PackageName = switches["install"][0],
                    Version = switches.ContainsKey("version") ? switches["version"][0] : null,
                    Mode = switches.ContainsKey("mode") ? switches["mode"][0] : null,
                };
                Packages = new[] { single };
            }
            else if (switches.ContainsKey("installmultiple"))
            {
                throw new NotImplementedException();
            }

            RebuildOnly = switches.Keys.Contains("rebuildonly");

            Refresh = switches.Keys.Contains("refresh");

            if (switches.Keys.Contains("output"))
                OutputPath = switches["output"][0];
        }
    }
}