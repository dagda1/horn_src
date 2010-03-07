namespace Horn.Core.Spec.Dependencies
{
    using System.IO;
    using System.Linq;
    using Core.Dependencies;
    using Xunit;

    public class when_updating_dependents : visual_studio_dependent_updater_context
    {

        protected override void Because()
        {
            dependentUpdater.Update(updaterContext);
        }


        //[Fact]
        public void should_load_all_solution_files_found_in_working_directory()
        {
            Assert.Equal(1, dependentUpdater.SolutionFiles.Count());
        }
        //[Fact]
        public void should_load_all_projects_found_in_solution_files()
        {
            Assert.Equal(1, dependentUpdater.ProjectFiles.Count());
        }
        //[Fact]
        public void should_update_refernce_version()
        {
            string contents = File.ReadAllText(projectPath);

            Assert.True(contents.Contains(VisualStudioDependentUpdaterDouble.NewReferenceName));
        }

    }

    public class when_updating_references_when_the_dependency_is_not_an_assembly : visual_studio_dependent_updater_context
    {

        protected override void Before_each_spec()
        {
            dependencyFilename = "bad.xml"; 
            base.Before_each_spec();
        }


        //[Fact]
        public void should_not_load_any_solution_files_found_in_working_directory()
        {
            dependentUpdater.Update(updaterContext);
            Assert.Equal(0, dependentUpdater.SolutionFiles.Count());
        }
        //[Fact]
        public void should_not_throw_exception()
        {
            dependentUpdater.Update(updaterContext);
        }

    }


    public class VisualStudioDependentUpdaterDouble : VisualStudioDependentUpdater
    {
        public const string NewReferenceName = "NewReference";

        protected override string GetVersionInfoFromAssembly(string filePath, string dependencyName)
        {
            return NewReferenceName;
        }
    }
}