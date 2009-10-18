using System;
using System.IO;
using Horn.Services.Core.Builder;
using Horn.Services.Core.Tests.Unit.SiteStructureSpecs;
using horn.services.core.Value;
using NUnit.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.BuildSpecs
{
    public class When_the_package_tree_is_scanned : BuilderSpecBase
    {
        [Test]
        public void Then_the_horn_object_graph_is_created()
        {
            Category category = siteStructureBuilder.Categories[0].Categories[0];

            AssertCategoryIntegrity(category);
        }

        [Test]
        public void Then_the_new_package_directory_is_copied_to_the_drop_directory()
        {
            fileSystemProvider.AssertWasCalled(x => x.CopyDirectory(Arg<string>.Is.TypeOf, Arg<string>.Is.TypeOf));
        }

        [Test]
        public void Then_the_result_of_each_bulid_is_zipped()
        {
            fileSystemProvider.ZipFolder(Arg<DirectoryInfo>.Is.TypeOf, Arg<DirectoryInfo>.Is.TypeOf, Arg<string>.Is.TypeOf);
        }

        protected override SiteStructureBuilder GetSiteBuilder()
        {
            return new SiteStructureBuilderDouble(metaDataSynchroniser, fileSystemProvider,
                                                  new DirectoryInfo(@"C:\").FullName);
        }
    }
}