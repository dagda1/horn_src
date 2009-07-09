using System;
using System.IO;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    using Rhino.Mocks;

    public class When_The_Build_Config_Reader_Receives_A_Request_For_A_Component : BaseDSLSpecification
    {
        private IDependencyResolver dependencyResolver;

        protected override void Before_each_spec()
        {
            dependencyResolver = CreateStub<IDependencyResolver>();
            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>())
                .Return(new SourceControlDouble(string.Empty));

            IoC.InitializeWith(dependencyResolver);

            GetTestBuildConfigsFolder();

            rootDirectory = GetTestBuildConfigsFolder();

            packageTree = new PackageTree(rootDirectory, null);
        }

        protected override void Because()
        {
            reader = new BooBuildConfigReader();
        }

        [Fact]
        public void Then_The_Config_Reader_Returns_The_Correct_MetaData()
        {
            var metaData = reader.SetDslFactory(packageTree).GetBuildMetaData("horn");

            AssertBuildMetaDataValues(metaData);
        }

    }

    public class When_SetDslFactory_Is_Not_Set : BaseDSLSpecification
    {
        protected override void Because()
        {
            reader = new BooBuildConfigReader();
        }

        [Fact]
        public void Then_An_Argument_Null_Exception_Is_Thrown()
        {
            Assert.Throws<ArgumentNullException>(() => reader.GetBuildMetaData("horn"));
        }

    }

    public class When_The_Build_File_Does_Not_Exist : BaseDSLSpecification
    {
        private string directoryWithNoBooFile;

        protected override void Because()
        {
            directoryWithNoBooFile = Path.Combine(DirectoryHelper.GetBaseDirectory(), "nonexistent");

            if (!Directory.Exists(directoryWithNoBooFile))
                Directory.CreateDirectory(directoryWithNoBooFile);

            rootDirectory = new DirectoryInfo(directoryWithNoBooFile);

            packageTree = new PackageTree(rootDirectory, null);

            reader = new BooBuildConfigReader();
        }

        [Fact]
        public void Then_The_Config_Reader_Throws_A_Custom_Exception()
        {
            Assert.Throws<MissingBuildFileException>(() => reader.SetDslFactory(packageTree).GetBuildMetaData("horn"));
        }

        protected override void After_each_spec()
        {
            try
            {
                if(Directory.Exists(directoryWithNoBooFile))
                    Directory.Delete(directoryWithNoBooFile, true);
            }
            catch
            {               
            }
        }
        
    }
}