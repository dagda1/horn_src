using Horn.Core.SCM;
using Xunit;
namespace Horn.Core.Spec.Unit.GetSpecs
{
    using GetOperations;

    public class When_a_get_request_is_made_to_retrieve_the_source : GetSpecificationBase
    {
        private string destinationPath;

        protected override void Because()
        {
            SourceControl.ClearDownLoadedPackages();

            get = new Get(fileSystemProvider);

            destinationPath = get.From(sourceControl)
                                .ExportTo(packageTree)
                                .RetrievePackage("horn").WorkingDirectory.FullName;
        }

        [Fact]
        public void Should_Retrieve_Source_From_VersionControl()
        {
            Assert.True(sourceControl.CheckOutWasCalled);
        }

        [Fact]
        public void Should_Return_The_Destination_Path()
        {
            Assert.NotEqual(string.Empty, destinationPath);
        }
    }
}