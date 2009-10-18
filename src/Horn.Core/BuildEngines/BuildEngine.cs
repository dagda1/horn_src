using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Horn.Core.Dependencies;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using log4net;
using Horn.Core.Extensions;

namespace Horn.Core.BuildEngines
{
    public class BuildEngine
    {
        public const string DefaultModeName = "default";

        private static readonly Dictionary<string, string> builtPackages = new Dictionary<string, string>();
        private IDependencyDispatcher dependencyDispatcher;
        private static readonly ILog log = LogManager.GetLogger(typeof(MSBuildBuildTool));


        public virtual string BuildFile { get; private set; }

        public virtual string BuildRootDirectory { get; set; }

        public virtual IBuildTool BuildTool { get; private set; }

        public IModeSettings DefaultModeSettings { get { return Modes[ DefaultModeName ]; } }

        public IModeSettings CurrentModeSettings { get; private set; }

        public virtual List<Dependency> Dependencies { get; protected set; }

        public virtual bool GenerateStrongKey { get; set; }

        public virtual FileInfo GetSnExe(IPackageTree packageTree)
        {
            var path = Path.Combine(packageTree.Root.CurrentDirectory.FullName, "buildengines");
            path = Path.Combine(path, "Sn");
            path = Path.Combine(path, "sn.exe");

            return new FileInfo(path);            
        }

        public virtual IDictionary<string, string> Parameters
        {
            get
            {
                if (CurrentModeSettings == null)
                {
                    return DefaultModeSettings.Parameters;
                }
                return CurrentModeSettings.Parameters;                
            }
        }

        public virtual string SharedLibrary { get; set; }

        public virtual IList<string> Tasks
        {
            get
            {
                if ( CurrentModeSettings == null )
                {
                    return new List<string>( DefaultModeSettings.Tasks );
                }
                return new List<string>( CurrentModeSettings.Tasks );
            }
        }

        public virtual FrameworkVersion Version { get; private set; }

        public virtual IDictionary<string, IModeSettings> Modes { get; private set;}

        public virtual void AssignParameters(string[] parameters)
        {
            CurrentModeSettings.AssignParameters( parameters );
        }

        public virtual void AssignTasks(string[] tasks)
        {
            CurrentModeSettings.AssignTasks( tasks );
        }

        public virtual void SetMode( string modeName )
        {
            if( modeName == DefaultModeName )
            {
                throw new ArgumentException( "You cannot explicity set the mode to the default mode ( " + DefaultModeName + ").  Use ResetMode() instead.");
            }

            if( CurrentModeSettings.Name != DefaultModeName )
            {
                throw new InvalidOperationException( "You cannot change modes when the current mode is not the default mode.  Ensure you call ResetMode() after each use of SetMode().");
            }

            if( Modes.ContainsKey( modeName ))
            {
                CurrentModeSettings = Modes[ modeName ];
                return;
            }

            var modeSettings = new ModeSettings( modeName );
            Modes.Add( modeName, modeSettings );
            CurrentModeSettings = modeSettings;
        }

        public virtual void ResetMode()
        {
            CurrentModeSettings = Modes[ DefaultModeName ];
        }

        public virtual void SetBuildMode( string modeName )
        {
            if( string.IsNullOrEmpty( modeName ) )
            {
                modeName = DefaultModeName;
            }

            if (Modes.ContainsKey(modeName))
            {
                CurrentModeSettings = Modes[modeName];
                return;
            }            
        }

        public virtual BuildEngine Build( IProcessFactory processFactory, IPackageTree packageTree )
        {
            return Build( processFactory, packageTree, DefaultModeName );
        }

        public virtual BuildEngine Build(IProcessFactory processFactory, IPackageTree packageTree, string mode)
        {
            if (builtPackages.ContainsKey(packageTree.Name))
                return this;

            SetBuildMode( mode );

            string pathToBuildFile = string.Format("{0}", GetBuildFilePath(packageTree).QuotePath());

            if (GenerateStrongKey)
                GenerateKeyFile(processFactory, packageTree);

            CopyDependenciesTo(packageTree);

            var cmdLineArguments = BuildTool.CommandLineArguments(pathToBuildFile, this, packageTree, Version);

            var pathToBuildTool = string.Format("{0}", BuildTool.PathToBuildTool(packageTree, Version).QuotePath());

            ProcessBuild(packageTree, processFactory, pathToBuildTool, cmdLineArguments);

            CopyArtifactsToBuildDirectory(packageTree);

            builtPackages.Add(packageTree.Name, packageTree.Name);

            return this;
        }

        public virtual void GenerateKeyFile(IProcessFactory processFactory, IPackageTree packageTree)
        {
            string strongKey = Path.Combine(packageTree.WorkingDirectory.FullName,
                                            string.Format("{0}.snk", packageTree.Name));

            string cmdLineArguments = string.Format("-k {0}", strongKey.QuotePath());

            var PSI = new ProcessStartInfo("cmd.exe")
                                       {
                                           RedirectStandardInput = true,
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           UseShellExecute = false
                                       };

            var sn = GetSnExe(packageTree);

            IProcess process = processFactory.GetProcess(sn.ToString().QuotePath(), cmdLineArguments, packageTree.WorkingDirectory.FullName);

            while (true)
            {
                string line = process.GetLineOrOutput();

                if (line == null)
                    break;

                log.Info(line);
            }

            process.WaitForExit();

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
            if (parts == ".")
                return sourceDirectory;

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

            try
            {
                process.WaitForExit();
            }
            catch (ProcessFailedException)
            {
                throw new BuildFailedException(string.Format("The build tool {0} failed building the {1} package", packageTree.BuildMetaData.BuildEngine.BuildTool, packageTree.Name));
            }
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
            Modes = new Dictionary<string, IModeSettings>();
            var defaultMode = new ModeSettings( DefaultModeName );
            Modes.Add( DefaultModeName, defaultMode );
            CurrentModeSettings = defaultMode;
        }
    }
}
