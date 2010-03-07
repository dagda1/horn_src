using Horn.Core.Utils.CmdLine;
using Horn.Spec.Framework.doubles;

namespace Horn.Framework.helpers
{
    public static class CommandLineHelper
    {
        public static ICommandArgs GetCommandLineArgs(string installName)
        {
            return new CommandArgsDouble(installName);
        }

        public static ICommandArgs GetCommandLineArgs(string installName, string version)
        {
            return new CommandArgsDouble(installName, version);
        }
    }
}