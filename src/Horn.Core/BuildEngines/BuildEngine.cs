using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Horn.Core.Dependencies;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using log4net;
using Horn.Core.extensions;

namespace Horn.Core.BuildEngines
{
    public class BuildEngine
    {
        private static readonly Dictionary<string, string> builtPackages = new Dictionary<string, string>();
        private IDependencyDispatcher dependencyDispatcher;
        private static readonly ILog log = LogManager.GetLogger(typeof(MSBuildBuildTool));

        public virtual string BuildFile { get; private set; }

        public virtual string BuildRootDirectory { get; set; }

        public virtual IBuildTool BuildTool { get; private set; }

        public virtual List<Dependency> Dependencies { get; protected set; }

        public virtual bool GenerateStrongKey { get; set; }

        public virtual Dictionary<string, string> Parameters { get; private set; }

        public virtual string SharedLibrary { get; set; }

        public virtual List<string> Tasks { get; private set; }

        public virtual FrameworkVersion Version { get; private set; }

        public virtual void AssignParameters(string[] parameters)
        {
            if ((parameters == null) || (parameters.Length == 0))
                return;

            Parameters = new Dictionary<string, string>();

            parameters.ForEach(x =>
                                   {
                                       var parts = x.Split('=');
                                        
                                       Parameters.Add(parts[0], parts[1]);
                                   });
        }

        public virtual void AssignTasks(string[] tasks)
        {
            Tasks = new List<string>(tasks);
        }

        public virtual BuildEngine Build(IProcessFactory processFactory, IPackageTree packageTree)
        {
            if (builtPackages.ContainsKey(packageTree.Name))
                return this;

            string pathToBuildFile = string.Format("\"{0}\"", GetBuildFilePath(packageTree));

            if (GenerateStrongKey)
                GenerateKeyFile(packageTree);

            CopyDependenciesTo(packageTree);

            var cmdLineArguments = BuildTool.CommandLineArguments(pathToBuildFile, this, packageTree, Version);

            var pathToBuildTool = string.Format("\"{0}\"", BuildTool.PathToBuildTool(packageTree, Version));

            ProcessBuild(packageTree, processFactory, pathToBuildTool, cmdLineArguments);

            if (BuildRootDirectory == ".")
                return this;

            CopyArtifactsToBuildDirectory(packageTree);

            builtPackages.Add(packageTree.Name, packageTree.Name);

            return this;
        }

        public virtual void GenerateKeyFile(IPackageTree packageTree)
        {
            string strongKey = Path.Combine(packageTree.WorkingDirectory.FullName,
                                            string.Format("{0}.snk", packageTree.Name));
                                            
            string commandLine = string.Format("{0} -k {1}", packageTree.Sn, strongKey);

            var PSI = new ProcessStartInfo("cmd.exe")
                                       {
                                           RedirectStandardInput = true,
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           UseShellExecute = false
                                       };

            Process p = Process.Start(PSI);
            StreamWriter SW = p.StandardInput;
            StreamReader SR = p.StandardOutput;
            SW.WriteLine(commandLine);
            SW.Close();
        }

        public virtual DirectoryInfo GetBuildDirectory(DirectoryInfo root)
        {
            if(!root.Exists)
                throw new DirectoryNotFoundException(string.Format("The build directory root {0} does not exist.", root.FullName));

            if ((root.GetFiles("*.dll").Length > 0) || (root.GetFiles("*.exe").Length > 0))
                return root;

            DirectoryInfo ret = null;

            foreach (var child in root.GetDirectories())
            {
                ret = GetBuildDirectory(child);

                if(ret != null)
                    break;
            }

            if(ret == null)
                throw new DirectoryNotFoundException(string.Format("no build files found at {0}.", root.FullName));

            return ret;
        }

        public virtual DirectoryInfo GetDirectoryFromParts(DirectoryInfo sourceDirectory, string parts)
        {
            return sourceDirectory.GetDirectoryFromParts(parts);
        }

        protected virtual void ProcessBuild(IPackageTree packageTree, IProcessFactory processFactory, string pathToBuildTool, string cmdLineArguments)
        {
            IProcess process = processFactory.GetProcess(pathToBuildTool, cmdLineArguments, packageTree.WorkingDirectory.FullName);

            while (true)
            {
                string line = process.GetLineOrOutput();

                if (line == null)
                    break;

                log.Info(line);
            }

            process.WaitForExit();
        }

        protected virtual void CopyArtifactsToBuildDirectory(IPackageTree packageTree)
        {

            DirectoryInfo buildDir = GetBuildDirectory(GetDirectoryFromParts(packageTree.WorkingDirectory, BuildRootDirectory));

            foreach (var file in buildDir.GetFiles())
            {
                var outputFile = Path.Combine(packageTree.Result.FullName, Path.GetFileName(file.FullName));

                CopyFileFromWorkingToResult(file, outputFile);
            }
        }

        protected virtual void CopyDependenciesTo(IPackageTree packageTree)
        {
            dependencyDispatcher.Dispatch(packageTree, Dependencies, SharedLibrary);
        }

        protected virtual void CopyFileFromWorkingToResult(FileInfo file, string outputFile)
        {
            if (File.Exists(outputFile))
                File.Delete(outputFile);

            File.Copy(file.FullName, outputFile, true);
        }

        protected virtual string GetBuildFilePath(IPackageTree tree)
        {
            var relativePath = BuildFile.Replace('/', '\\');

            return Path.Combine(tree.WorkingDirectory.FullName, relativePath);
        }

        public BuildEngine(IBuildTool buildTool, string buildFile, FrameworkVersion version, IDependencyDispatcher dependencyDispatcher)
        {
            BuildTool = buildTool;
            BuildFile = buildFile;
            Version = version;
            Dependencies = new List<Dependency>();
            this.dependencyDispatcher = dependencyDispatcher;
        }
    }
}
