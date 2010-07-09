using System.Collections.Generic;

namespace Horn.Core.Utils.CmdLine
{
    /// <summary>
    /// Used to signify that the help switch was passed
    /// </summary>
    public class HelpReturnValue : Dictionary<string, IList<string>>
    {
    }
}