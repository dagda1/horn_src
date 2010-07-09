using System;
using System.IO;
using Horn.Core.Utils.Framework;
using Xunit;

namespace Horn.Core.Spec.Integration.Utils
{
    public class When_Framework_Version_35_Is_Requested : Specification
    {
        private string msbuildPath;

        protected override void Because()
        {
            msbuildPath = FrameworkLocator.Instance[FrameworkVersion.FrameworkVersion35].MSBuild.AssemblyPath;
        }

        [Fact]
        public void Then_Framework_35_Path_Is_Returned()
        {
            log.Info(msbuildPath);

            Assert.True(File.Exists(msbuildPath));
        }
    }
}