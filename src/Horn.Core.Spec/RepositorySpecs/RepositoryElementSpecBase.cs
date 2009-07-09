using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Spec.Doubles;
using Horn.Core.Spec.helpers;
using Rhino.Mocks;

namespace Horn.Core.Spec.RepositorySpecs
{
    public abstract class RepositoryElementSpecBase : Specification
    {
        protected RepositoryElementStub repositoryElement;
        protected IPackageTree packageTree;
        protected IGet get;
        protected IDependencyResolver dependencyResolver;
        protected MockRepository mockRepository = new MockRepository();

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempPackageTree();

            get = MockRepository.GenerateStub<IGet>();

            repositoryElement = new RepositoryElementStub("castle", "Tools", "Tools");

            dependencyResolver = CreateStub<IDependencyResolver>();

            dependencyResolver.Stub(x => x.Resolve<IBuildConfigReader>()).Return(new BooBuildConfigReader());

            var svn = new SVNSourceControl("http://svnserver/trunk");

            get.Stub(x => x.From(svn)).Return(get);

            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(svn);

            IoC.InitializeWith(dependencyResolver);
        }
    }
}