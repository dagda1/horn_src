using System.IO;

namespace Horn.Core.Utils.Framework
{
    public class MSBuild
    {

        public string AssemblyPath { get; private set; }



        public MSBuild(string frameworkPath)
        {
            AssemblyPath = Path.Combine(frameworkPath, "MSBuild.exe");
        }



    }

}