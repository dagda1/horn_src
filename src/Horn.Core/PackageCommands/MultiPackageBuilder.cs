using System;

using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageCommands
{
	public class MultiPackageBuilder
		: PackageBuilder
	{
		public MultiPackageBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs)
			: base(get, processFactory, commandArgs)
		{
		}

		public override void Execute(PackageStructure.IPackageTree packageTree)
		{
			throw new NotImplementedException();
		}
	}
}