using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.Spec.Stubs
{
    public class PackageBuilderWithOnlyPrebuildStub : PackageBuilder
    {
        private readonly IBuildMetaData buildMetaData;

        public override void Execute(IPackageTree packageTree)
        {
            ExecutePrebuildCommands(buildMetaData, packageTree);
        }

        public PackageBuilderWithOnlyPrebuildStub(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs, IBuildMetaData buildMetaData) : base(get, processFactory, commandArgs)
        {
            this.buildMetaData = buildMetaData;
        }
    }
}