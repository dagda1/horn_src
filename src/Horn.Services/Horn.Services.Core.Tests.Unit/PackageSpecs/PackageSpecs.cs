using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using horn.services.core.Value;
using Horn.Spec.Framework;
using Horn.Spec.Framework.helpers;
using NUnit.Framework;

namespace Horn.Services.Core.Tests.Unit.PackageSpecs
{
    public class When_mapping_a_build_package_meta_data_to_a_package_object : ContextSpecification 
    {
        private Package package;

        private IBuildMetaData buildMetaData;
        private Category nhibernate;

        protected override void establish_context()
        {
            var horn = new Category(null, "orm");

            nhibernate = new Category(horn, "nhibernate");

            buildMetaData = TreeHelper.GetPackageTreeParts(new List<Dependency>());

            buildMetaData.InstallName = "nhibernate";

            buildMetaData.Version = "trunk";

            buildMetaData.ProjectInfo.Add("forum", "http://groups.google.co.uk/group/nhusers?hl=en");
        }

        protected override void because()
        {
            package = new Package(nhibernate, buildMetaData);
        }

        [Test]
        public void Then_the_package_details_are_recorded()
        {
            Assert.That(package.Name, Is.EqualTo("nhibernate"));

            Assert.That(package.Version, Is.EqualTo("trunk"));

            Assert.That(package.MetaData.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Then_the_contents_of_the_build_are_recorded()
        {
            package.SetContents(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory), new FileInfo(@"C:\output.zip"));

            Assert.That(package.ZipFileName.Name, Is.EqualTo("output.zip"));

            Assert.That(package.Contents.Count, Is.GreaterThan(0));
        }
    }
}