using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class NAntBuildTool : IBuildTool
    {
        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            return string.Format(" {0} -t:net-{1} -buildfile:{2} {3}", GenerateTasks(buildEngine.Tasks), GetFrameworkVersionForBuildTool(version), pathToBuildFile, GenerateParameters(buildEngine.Parameters)).Trim();
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            switch (version)
            {
                case FrameworkVersion.FrameworkVersion2:
                    return "2.0";
                case FrameworkVersion.FrameworkVersion35:
                    return "3.5";
            }

            throw new InvalidEnumArgumentException("Invalid Framework Version paased to NAntBuildTool.GetFrameworkVersion", (int)version, typeof(FrameworkVersion));
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            var path = Path.Combine(packageTree.Root.CurrentDirectory.FullName, "buildengines");

            path = Path.Combine(path, "Nant");
            path = Path.Combine(path, "Nant");
            path = Path.Combine(path, "NAnt.exe");

            return new FileInfo(path).FullName;
        }

        private string GenerateParameters( IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Keys.Count == 0)
                return String.Empty;

            var stringBuilder = new StringBuilder();

            foreach (var key in parameters.Keys)
                stringBuilder.AppendFormat("-D:{0}={1} ", key, parameters[key]);

            return stringBuilder.ToString();
        }

        private string GenerateTasks(IEnumerable<string> tasks)
        {
            if( tasks == null )
            {
                return String.Empty;
            }

            var tasksArgument = String.Empty;
            tasks.ForEach( task => tasksArgument += String.Format("{0} ", task) );
            return tasksArgument;
        }
    }
}
