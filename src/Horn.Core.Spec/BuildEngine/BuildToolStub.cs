using System;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core.Spec.BuildEngineSpecs
{
    public class BuildToolStub : IBuildTool
    {
        public string PathToBuildFile { get; private set; }

        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            Console.WriteLine(pathToBuildFile);
            Console.WriteLine(buildEngine);
            Console.WriteLine(packageTree);
            Console.WriteLine(version);

            return string.Empty;
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            Console.WriteLine(version);

            return "3.5";
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            Console.WriteLine(version);

            return string.Empty;
        }
    }
}