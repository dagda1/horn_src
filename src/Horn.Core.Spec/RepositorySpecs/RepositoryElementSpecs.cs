using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.RepositorySpecs
{
   public class When_the_a_repository_element_is_requested : RepositoryElementSpecBase
   {
        protected override void Because()
        {
            mockRepository.Playback();

            repositoryElement.PrepareRepository(packageTree, get);
        }

        [Fact]
        public void Then_the_repository_should_be_created()
        {
            get.AssertWasCalled(x => x.ExportTo(packageTree.RetrievePackage("castle")));
        }
    }

    public class When_the_repository_has_been_created : RepositoryElementSpecBase
    {
        protected override void Because()
        {
            mockRepository.Playback();

            repositoryElement.PrepareRepository(packageTree, get).Export();            
        }

        [Fact]
        public void Then_the_parts_should_be_exported_to_their_destination()
        {
            
        }
    }

}