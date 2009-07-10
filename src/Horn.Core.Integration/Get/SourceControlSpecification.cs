using Horn.Core.SCM;
using Xunit;

namespace Horn.Core.Spec.Integration.Get
{
    public class When_Horn_Is_In_The_BuildMetaData : SourceControlTestBase
    {
        public const string HORN_URL = "https://hornget.googlecode.com/svn/trunk";

        protected override void Because()
        {
            SourceControl.ClearDownLoadedPackages();

            SourceControl svn = new SVNSourceControl(HORN_URL);

            svn.Export(packageTree);
        }

        [Fact]
        public void Then_The_Horn_Source_Is_Downloaded()
        {
            Assert.InRange(packageTree.WorkingDirectory.GetDirectories().Length, 1, 20);
        }
    }
}