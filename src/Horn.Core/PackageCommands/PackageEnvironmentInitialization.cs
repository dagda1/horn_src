using System;

using Horn.Core.BuildEngines;
using Horn.Core.SCM;

namespace Horn.Core.PackageCommands
{
    public static class PackageEnvironmentInitialization
    {
        public static void InitialiseForClearEnvironment()
        {
            BuildEngine.ClearBuiltPackages();
            SourceControl.ClearDownLoadedPackages();
        }
    }
}