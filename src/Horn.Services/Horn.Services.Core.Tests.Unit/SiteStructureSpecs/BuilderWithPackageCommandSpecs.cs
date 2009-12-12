using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Services.Core.Builder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.SiteStructureSpecs
{
    public class When_the_builder_discovers_a_package : BuilderSpecBase
    {
        [Test]
        public void Then_the_package_is_built()
        {
            packageBuilder.AssertWasCalled(x => x.Execute(Arg<IPackageTree>.Is.TypeOf));
        }

        protected override SiteStructureBuilder GetSiteBuilder()
        {
            return new SiteStructureBuilder(metaDataSynchroniser, fileSystemProvider, new DirectoryInfo(@"C:\").FullName);
        }
    }
}