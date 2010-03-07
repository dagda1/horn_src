namespace Horn.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BuildEngines;
    using PackageStructure;
    using Utils.Framework;

    public class BatchBuildTool : IBuildTool
    {
        private string buildFile;

        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            buildFile = pathToBuildFile;

            return GenerateParameters(buildEngine.Parameters);
        
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return buildFile;
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            return "batch files don't care";
        }


        private string GenerateParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Keys.Count == 0)
                return string.Empty;

            var stringBuilder = new StringBuilder();

            foreach (var key in parameters.Keys)
                stringBuilder.AppendFormat("-D:{0}={1} ", key, parameters[key]);

            return stringBuilder.ToString();
        }
    }
}
