using System.IO;
using Horn.Core.Dsl;
using Horn.Core.SCM;
using Horn.Core.Spec.helpers;
using Horn.Framework.helpers;
using Rhino.Mocks;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.Unit.dsl;
using Xunit;

namespace Horn.Core.Spec.Unit.HornTree
{
    public class When_Given_The_Package_Root_Directory : DirectorySpecificationBase
    {
        private IPackageTree rootTree;

        protected override void Because()
        {
            rootTree = new PackageTree(rootDirectory, null);
        }

        [Fact]
        public void Then_the_tree_root_is_the_root()
        {
            Assert.True(rootTree.IsRoot);
        }

        [Fact]
        public void Then_the_root_will_have_more_than_Children()
        {
            Assert.True(rootTree.Children.Length > 1);
        }

        [Fact]
        public void Then_The_CurrentDirectory_is_the_Root_Directory()
        {
            Assert.Equal(rootDirectory.FullName, rootTree.CurrentDirectory.FullName);
        }
    }

    public class When_A_PackageTree_Node_Contains_A_Build_File : DirectorySpecificationBase
    {

        private IPackageTree hornTree;


        protected override void Because()
        {
            hornTree = new PackageTree(rootDirectory, null);
        }


        [Fact]
        public void Then_The_Node_Will_have_a_Child()
        {
            Assert.Equal(1, hornTree.Children[1].Children.Length);
        }
        [Fact]
        public void Then_The_Node_Is_A_Build_Node()
        {
            Assert.True(hornTree.Children[1].Children[0].IsBuildNode);
        }

    }

    public class When_Given_A_Request_For_A_Build_File : DirectorySpecificationBase
    {

        private IPackageTree hornTree;
        private IDependencyResolver dependencyResolver;


        protected override void Before_each_spec()
        {
            IBuildConfigReader buildConfigReader = new BooBuildConfigReader();

            dependencyResolver = CreateStub<IDependencyResolver>();

            dependencyResolver.Stub(x => x.Resolve<IBuildConfigReader>()).Return(buildConfigReader);

            var svn = new SVNSourceControl("http://svnserver/trunk");

            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(svn);

            IoC.InitializeWith(dependencyResolver);
        }

        protected override void Because()
        {
            hornTree = TreeHelper.GetTempPackageTree();
        }


        [Fact]
        public void Then_Horn_Retrieves_The_Build_File_From_The_Structure()
        {
            var metaData = hornTree.RetrievePackage("horn").GetBuildMetaData("horn");

            BaseDSLSpecification.AssertBuildMetaDataValues(metaData);
        }

    }

    public class When_Build_Nodes_Are_Requested : DirectorySpecificationBase
    {

        private IPackageTree hornTree;


        protected override void Because()
        {
            hornTree = new PackageTree(rootDirectory, null);
        }


        [Fact]
        public void Then_A_List_Of_Build_Nodes_Are_Returned()
        {
            Assert.True(hornTree.BuildNodes().Count > 0);

            Assert.Equal("horn", hornTree.BuildNodes()[0].Name);
        }

    }

    public class When_Retrieve_Does_Not_Return_A_Package : DirectorySpecificationBase
    {

        private IPackageTree hornTree;


        protected override void Because()
        {
            hornTree = new PackageTree(rootDirectory, null);
        }


        [Fact]
        public void Then_A_Null_Package_Tree_Object_Is_Returned()
        {
            Assert.IsType<NullPackageTree>(hornTree.RetrievePackage("unkownpackage"));
        }

        [Fact]
        public void Then_A_Null_Build_Meta_Data_Object_Is_Returned()
        {
            Assert.IsType<NullBuildMetaData>(hornTree.RetrievePackage("unkonwnpackage").GetBuildMetaData("unkonwnpackage"));
        }

    }

    public class When_the_package_tree_root_directory_does_not_exist : DirectorySpecificationBase
    {

        private IPackageTree packageTree;


        protected override void Because()
        {
            packageTree = TreeHelper.GetTempEmptyPackageTree();
        }


        [Fact]
        public void Then_the_meta_data_synchroniser_returns_false()
        {
            Assert.False(packageTree.Exists);
        }

    }

    public class When_the_package_tree_root_directory_exists_but_there_are_no_build_files : DirectorySpecificationBase
    {

        private IPackageTree packageTree;


        protected override void Because()
        {
            packageTree = TreeHelper.GetTempEmptyPackageTree();

            packageTree.CreateRequiredDirectories();
        }


        [Fact]
        public void Then_the_meta_data_synchroniser_returns_false()
        {
            Assert.False(packageTree.Exists);
        }

    }

    public class When_the_root_directory_exists_and_contains_build_files : DirectorySpecificationBase
    {

        private IPackageTree packageTree;


        protected override void Because()
        {
            packageTree = TreeHelper.GetTempPackageTree();
        }


        [Fact]
        public void Then_the_meta_data_synchroniser_returns_true()
        {
            Assert.True(packageTree.Exists);
        }

    }

    public class When_a_package_revision_data_does_not_exist : Specification
    {

        private IPackageTree package;
        private IRevisionData revisionData;


        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PACKAGE_WITHOUT_REVISION);
        }

        protected override void Because()
        {
            revisionData = package.GetRevisionData();
        }


        [Fact]
        public void Then_a_new_revsion_data_is_returned()
        {
            Assert.Equal(revisionData.Revision, "0");
        }

    }

    public class When_a_package_revision_data_does_exist : Specification
    {

        private IPackageTree package;
        private IRevisionData revisionData;


        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PACKAGE_WITH_REVISION);
        }

        protected override void Because()
        {
            revisionData = package.GetRevisionData();
        }


        [Fact]
        public void Then_the_existing_revision_data_is_returned()
        {
            Assert.Equal(revisionData.Revision, "1");
        }

    }
}