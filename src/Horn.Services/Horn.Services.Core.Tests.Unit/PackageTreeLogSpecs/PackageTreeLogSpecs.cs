using Horn.Core.Extensions;
using Horn.Services.Core.Log;
using Horn.Spec.Framework;
using NUnit.Framework;
using Horn.Services.Core.Tests.Unit.Helpers;

namespace Horn.Services.Core.Tests.Unit.PackageTreeDetailsSpecs
{
    public class When_a_package_tree_is_created : ContextSpecification 
    {
        private PackageTreeLog packageTreeLog;

        protected override void establish_context()
        {
        }

        protected override void because()
        {
            packageTreeLog = new PackageTreeLog(PackageTreeHelper.GetFakePackageTree());
        }

        [Test]
        public void Then_the_package_tree_location_is_recorded()
        {
            Assert.That(packageTreeLog.Location.FullName.Length, Is.GreaterThan(0));
        }

        [Test]
        public void Should_serialise_and_deserialise()
        {
            var xml = packageTreeLog.ToDataContractXml<PackageTreeLog>();

            var log = SerialisationExtensions.DescrialiseContractXml<PackageTreeLog>(xml);

            Assert.That(log.Location.FullName.Length, Is.GreaterThan(0));
        }
    }
}