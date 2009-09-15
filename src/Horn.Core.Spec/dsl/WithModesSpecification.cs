using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.SCM;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_the_with_task_contains_debug_and_release_modes : BaseDSLSpecification
    {
        private const string DebugModeName = "debug";
        private const string ReleaseModeName = "release";

        private BooConfigReader configReader;
        protected DslFactory factory;
        private IDependencyResolver dependencyResolver;

        protected override void Before_each_spec()
        {
            dependencyResolver = CreateStub<IDependencyResolver>();
            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>())
                .Return(new SVNSourceControl(string.Empty));

            IoC.InitializeWith(dependencyResolver);

            var engine = new ConfigReaderEngine();

            factory = new DslFactory { BaseDirectory = DirectoryHelper.GetBaseDirectory() };
            factory.Register<BooConfigReader>(engine);
        }

        protected override void After_each_spec()
        {
            IoC.InitializeWith(null);
        }

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornwithmodes.boo");
            configReader.Prepare();
        }

        [Fact]
        public void The_default_mode_is_created()
        {
            Assert.Contains( BuildEngine.DefaultModeName, configReader.BuildMetaData.BuildEngine.Modes.Keys );
        }

        [Fact]
        public void The_debug_mode_is_parsed()
        {
            Assert.Contains( DebugModeName, configReader.BuildMetaData.BuildEngine.Modes.Keys );
        }

        [Fact]
        public void The_release_mode_is_parsed()
        {
            Assert.Contains( ReleaseModeName, configReader.BuildMetaData.BuildEngine.Modes.Keys );
        }

        [Fact]
        public void The_default_mode_contains_tasks_not_in_a_mode_block()
        {
            var expectedTasks = new[] {"build", "quick"};
            var actualTasks = configReader.BuildMetaData
                                  .BuildEngine.Modes[ BuildEngine.DefaultModeName ]
                                  .Tasks;
            Assert.Equal( expectedTasks.Length, actualTasks.Count );
            for( var taskIndex = 0; taskIndex < expectedTasks.Length; taskIndex++ )
            {
                Assert.Equal( expectedTasks[taskIndex], actualTasks[taskIndex] );
            }
        }

        [Fact]
        public void The_default_mode_contains_parameters_not_in_a_mode_block()
        {
            var expectedParameters = new[]
                                         {
                                             new KeyValuePair<string, string>( "testrunner", "NUnit" ),
                                             new KeyValuePair<string, string>( "common.testrunner.enabled", "true" ),
                                             new KeyValuePair<string, string>( "common.testrunner.failonerror", "true" ),
                                             new KeyValuePair<string, string>( "build.msbuild", "true" )
                                         };
            var actualParameters = configReader.BuildMetaData
                                       .BuildEngine.Modes[ BuildEngine.DefaultModeName ]
                                       .Parameters;
            Assert.Equal(expectedParameters.Length, actualParameters.Count);
            for (var parameterIndex = 0; parameterIndex < expectedParameters.Length; parameterIndex++)
            {
                var expectedParameter = expectedParameters[ parameterIndex ];
                Assert.Equal( expectedParameter.Value, actualParameters[expectedParameter.Key] );
            }
        }

        [Fact]
        public void The_debug_mode_contains_the_tasks_in_the_mode_debug_block()
        {
            var expectedTasks = new[] {"debug"};
            var actualTasks = configReader.BuildMetaData
                                  .BuildEngine.Modes[ DebugModeName ]
                                  .Tasks;
            Assert.Equal(expectedTasks.Length, actualTasks.Count);
            for (var taskIndex = 0; taskIndex < expectedTasks.Length; taskIndex++)
            {
                Assert.Equal(expectedTasks[taskIndex], actualTasks[taskIndex]);
            }
        }

        [Fact]
        public void The_debug_mode_contains_the_parameters_in_the_mode_debug_block()
        {
            var expectedParameters = new[]
                                         {
                                             new KeyValuePair<string, string>( "sign", "false" ),
                                         };
            var actualParameters = configReader.BuildMetaData
                                       .BuildEngine.Modes[ DebugModeName ]
                                       .Parameters;
            Assert.Equal(expectedParameters.Length, actualParameters.Count);
            for (var parameterIndex = 0; parameterIndex < expectedParameters.Length; parameterIndex++)
            {
                var expectedParameter = expectedParameters[parameterIndex];
                Assert.Equal(expectedParameter.Value, actualParameters[expectedParameter.Key]);
            }
        }

        [Fact]
        public void The_release_mode_contains_the_tasks_in_the_mode_release_block()
        {
            var expectedTasks = new[] {"release"};
            var actualTasks = configReader.BuildMetaData
                                  .BuildEngine.Modes[ ReleaseModeName ]
                                  .Tasks;
            Assert.Equal(expectedTasks.Length, actualTasks.Count);
            for (var taskIndex = 0; taskIndex < expectedTasks.Length; taskIndex++)
            {
                Assert.Equal(expectedTasks[taskIndex], actualTasks[taskIndex]);
            }
        }

        [Fact]
        public void The_release_mode_contains_the_parameters_in_the_mode_release_block()
        {
            var expectedParameters = new[]
                                         {
                                             new KeyValuePair<string, string>( "sign", "true" ),
                                         };
            var actualParameters = configReader.BuildMetaData
                                       .BuildEngine.Modes[ ReleaseModeName ]
                                       .Parameters;
            Assert.Equal(expectedParameters.Length, actualParameters.Count);
            for (var parameterIndex = 0; parameterIndex < expectedParameters.Length; parameterIndex++)
            {
                var expectedParameter = expectedParameters[parameterIndex];
                Assert.Equal(expectedParameter.Value, actualParameters[expectedParameter.Key]);
            }
        }
    }
}
