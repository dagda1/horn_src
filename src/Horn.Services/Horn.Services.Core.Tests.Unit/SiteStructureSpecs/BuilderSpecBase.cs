using System.IO;
using Horn.Core.Utils.IoC;
using Horn.Services.Core.Tests.Unit.Helpers;
using Horn.Spec.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.PackageTreeBuilderSpecs
{
    public abstract class BuilderSpecBase : ContextSpecification 
    {
        protected DirectoryInfo hornDirectory;
        protected IDependencyResolver dependencyResolver;

        public override void before_each_spec()
        {
            hornDirectory = FileSystemHelper.GetFakeDummyHornDirectory();

            dependencyResolver = MockRepository.GenerateStub<IDependencyResolver>();

            IoC.InitializeWith(dependencyResolver);
        }
    }
}