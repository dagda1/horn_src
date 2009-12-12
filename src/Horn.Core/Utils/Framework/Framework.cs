using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using log4net;

namespace Horn.Core.Utils.Framework
{
    public enum FrameworkVersion
    {
        FrameworkVersion2,
        FrameworkVersion35
    }

    public class Framework
    {

        private static readonly IDictionary<FrameworkVersion, string> assemblyPaths = new Dictionary<FrameworkVersion, string>();
        private static readonly ILog log = LogManager.GetLogger(typeof (Framework));


        public MSBuild MSBuild
        {
            get { return new MSBuild(assemblyPaths[Version]); }
        }

        public FrameworkVersion Version { get; private set; }



        static Framework()
        {
            const string Index = "\\Microsoft.NET\\";

            //HACK: Is there a better way to determine the Correct framework path
            var currentVersion = RuntimeEnvironment.GetRuntimeDirectory();

            Console.WriteLine("Runtime directory = {0}", RuntimeEnvironment.GetRuntimeDirectory());

            var frameworkRoot = new DirectoryInfo(currentVersion.Substring(0, currentVersion.LastIndexOf(Index) + Index.Length));

            DirectoryInfo frameworkDir;

            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
                frameworkDir = new DirectoryInfo(Path.Combine(frameworkRoot.FullName, "Framework"));
            else
                frameworkDir = new DirectoryInfo(Path.Combine(frameworkRoot.FullName, "Framework64"));

            assemblyPaths.Add(FrameworkVersion.FrameworkVersion2, Path.Combine(frameworkDir.FullName, "v2.0.50727"));
            assemblyPaths.Add(FrameworkVersion.FrameworkVersion35, Path.Combine(frameworkDir.FullName, "v3.5"));
        }



        public Framework(FrameworkVersion version)
        {
            Version = version;
        }



    }
}