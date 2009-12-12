using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageCommands
{
    public interface IPackageCommand
    {
        void Execute(IPackageTree packageTree);
    }
}