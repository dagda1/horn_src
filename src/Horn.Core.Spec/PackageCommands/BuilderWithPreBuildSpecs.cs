using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.SCM;
using Horn.Core.Spec.Unit.GetSpecs;
using Horn.Spec.Framework.doubles;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.PackageCommands
{
    public class When_the_meta_data_has_a_prebuild_list : GetSpecificationBase
    {
        private string testFile;
        private PackageBuilder packageBuilder;
        private MockRepository mockRepository;

        protected override void Before_each_spec()
        {
            mockRepository = new MockRepository();

            testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt");

            DeleteTestFile();

            var cmds = new List<string> { string.Format("@echo \"hello\" > {0}", Path.GetFileName(testFile)) };

            packageTree = new PackageTreeStub(TreeHelper.GetPackageTreeParts(new List<Dependency>(), cmds), "log4net", false);

            get = MockRepository.GenerateStub<IGet>();

            get.Stub(x => x.From(new SVNSourceControl("url"))).Return(get);

            get.Stub(x => x.ExportTo(packageTree)).Return(packageTree);

            packageBuilder = new PackageBuilderStub(get, new DiagnosticsProcessFactory(), new CommandArgsDouble("log4net", true));
        }

        protected override void After_each_spec()
        {
            DeleteTestFile();
        }

        protected override void Because()
        {
            mockRepository.Playback();

            packageBuilder.Execute(packageTree);
        }

        private void DeleteTestFile()
        {
            if (File.Exists(testFile))
                File.Delete(testFile);
        }

        [Fact]
        public void Then_the_prebuild_commands_are_executed()
        {
            Assert.True(File.Exists(testFile));
        }
    }
}