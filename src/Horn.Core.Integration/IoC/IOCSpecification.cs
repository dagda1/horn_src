using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.Integration.IoC;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Tree.MetaDataSynchroniser;
using Xunit;

namespace Horn.Core.Spec.Integration
{
    using Dependencies;

    public class When_An_IBuildConfigReader_Is_Requested_From_The_Container : IoCSpecificationBase
    {
        [Fact]
        public void Then_A_Build_Config_Reader_Is_Returned()
        {
            Assert.IsAssignableFrom<IBuildConfigReader>(IoC.Resolve<IBuildConfigReader>());
        }
    }

    public class When_A_SourceControl_Is_Requested_From_The_Container : IoCSpecificationBase
    {
        [Fact]
        public void Then_The_Build_Config_Reader_Is_Returned()
        {
            Assert.IsAssignableFrom<SVNSourceControl>(IoC.Resolve<SVNSourceControl>());
        }
    }

    public class When_An_Install_Key_Is_Requested_From_The_Container : IoCSpecificationBase
    {
        [Fact]
        public void Then_A_Package_Builder_Is_Returned()
        {
            Assert.IsAssignableFrom<PackageBuilder>(IoC.Resolve<IPackageCommand>("install"));
        }
    }

    public class When_An_IProcessFactory_Is_Requested_From_The_Contaier : IoCSpecificationBase
    {
        [Fact]
        public void Then_A_ProcessFactory_Is_Returned()
        {
            Assert.IsAssignableFrom<DiagnosticsProcessFactory>(IoC.Resolve<IProcessFactory>());
        }
    }

    public class When_An_IDependencyDispatcher_Is_Requested_From_The_Contaier : IoCSpecificationBase
    {
        [Fact]
        public void Then_A_ProcessFactory_Is_Returned()
        {
            Assert.IsAssignableFrom<DependencyDispatcher>(IoC.Resolve<IDependencyDispatcher>());
        }
    }

    public class When_The_Package_Tree_Root_Is_Requested_From_The_Contaier : IoCSpecificationBase
    {
        [Fact]
        public void Then_A_ProcessFactory_Is_Returned()
        {
            Assert.IsAssignableFrom<IPackageTree>(IoC.Resolve<IPackageTree>());
        }
    }
}