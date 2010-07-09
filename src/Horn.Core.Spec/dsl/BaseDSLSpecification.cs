using System.Collections.Generic;
using System.IO;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils.Framework;
using Horn.Framework.helpers;
using Horn.Spec.Framework.Extensions;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public abstract class BaseDSLSpecification : Specification
    {
        protected const string Description = "A .NET build and dependency manager";
        protected const string SvnUrl = "http://hornget.googlecode.com/svn/trunk/";
        protected const string FileName = "horn";
        protected const string BuildFile = "src/horn.sln";
        public static readonly Dictionary<string, object> MetaData = new Dictionary<string, object> { { "homepage", "http://code.google.com/p/scotaltdotnet/" }, { "forum", "http://groups.google.co.uk/group/horn-development?hl=en" }, { "contrib", false} };
        public  static readonly List<string> Tasks = new List<string> {"build"};
        public const string OutputDirectory = "Output";
        protected DirectoryInfo rootDirectory;
        protected IPackageTree packageTree;
        protected IBuildConfigReader reader;

        public static IBuildMetaData GetBuildMetaDataInstance()
        {
            BooConfigReader ret = new ConfigReaderDouble();

            ret.description(Description);
            ret.BuildMetaData.BuildEngine = new BuildEngines.BuildEngine(new MSBuildBuildTool(), BuildFile, FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            ret.BuildMetaData.SourceControl = new SVNSourceControl(SvnUrl);

            foreach (var item in MetaData)
                ret.BuildMetaData.ProjectInfo.Add(item.Key, item.Value);

            ret.BuildMetaData.BuildEngine.AssignTasks(Tasks.ToArray());
            ret.BuildMetaData.BuildEngine.BuildRootDirectory = OutputDirectory;
            ret.BuildMetaData.BuildEngine.SharedLibrary = ".";
            ret.BuildMetaData.BuildEngine.GenerateStrongKey = true;

            return ret.BuildMetaData;
        }

        public static void AssertBuildMetaDataValues(IBuildMetaData metaData)
        {
            Assert.IsAssignableFrom<SVNSourceControl>(metaData.SourceControl);

            Assert.Equal(SvnUrl, metaData.SourceControl.Url);

            Assert.IsAssignableFrom<MSBuildBuildTool>(metaData.BuildEngine.BuildTool);

            //TODO: Uncomment.  The metadata is currently not being parsed from the BooConfigReader
            //Assert.Equal(MetaData.Count, metaData.ProjectInfo.Count);
            //MetaData.ForEach(x => Assert.Contains(x, metaData.ProjectInfo));

            Assert.Equal(BuildFile, metaData.BuildEngine.BuildFile);

            Assert.Equal(OutputDirectory, metaData.BuildEngine.BuildRootDirectory);

            Assert.Equal(".", metaData.BuildEngine.SharedLibrary);
        }

        protected DirectoryInfo GetTestBuildConfigsFolder()
        {
            var pathToConfigs = Path.Combine(DirectoryHelper.GetBaseDirectory().ToLower().ResolvePath(),
                                             "BuildConfigs\\Horn");

            return new DirectoryInfo(pathToConfigs);
        }
    }
}
