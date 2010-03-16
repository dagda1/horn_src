using Boo.Lang.Compiler.Steps;
using Boo.Lang.Compiler;
using Rhino.DSL;

namespace Horn.Core.Dsl
{
    public class ConfigReaderEngine : DslEngine
    {
        protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls)
        {
            pipeline.Insert(1, new ImplicitBaseClassCompilerStep(typeof(BooConfigReader), "Prepare", "Horn.Core.Dsl"));
            pipeline.InsertBefore(typeof(ProcessMethodBodiesWithDuckTyping), new RightShiftToMethodCompilerStep());
            pipeline.Insert(2, new UnderscoreNamingConventionsToPascalCaseCompilerStep());
            pipeline.Insert(3, new UseSymbolsStep());            
        }
    }
}