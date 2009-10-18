using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using horn.services.core.Value;
using Horn.Spec.Framework;
using Horn.Spec.Framework.helpers;
using NUnit.Framework;

namespace Horn.Services.Core.Tests.Unit.PackageSpecs
{
    public class When_mapping_a_build_package_meta_data_to_a_value_object : ContextSpecification 
    {
        private Package package;

        private IBuildMetaData buildMetaData;
        private Category nhibernate;

        protected override void establish_context()
        {
            var horn = new Category(null, "horn");

            nhibernate = new Category(horn, "orm");


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
        public void Then_the_value_object_is_created()
        {
            Assert.That(package.Name, Is.EqualTo("nhibernate"));

            Assert.That(package.Version, Is.EqualTo("trunk"));

            Assert.That(package.MetaData.Count, Is.GreaterThan(0));

            Assert.That(package.Url, Is.EqualTo("orm/nhibernate-trunk"));
        }
    }
}