using System;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

namespace Horn.Spec.Framework.Stubs
{
    public class PackageBuilderStub : PackageBuilder
    {
        protected override void BuildSource(IPackageTree nextTree, IBuildMetaData nextMetaData)
        {
            Console.WriteLine(string.Format("Building {0}", nextMetaData.InstallName));
        }

        public PackageBuilderStub(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs)
            : base(get, processFactory, commandArgs)
        {
        }
    }
}