using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.SCM;

namespace Horn.Core.Dsl
{
    public interface IBuildMetaData
    {
        string InstallName { get; set; }
        string Description { get; set; }
        BuildEngine BuildEngine { get; set; }
        SourceControl SourceControl { get; set; }

        List<SourceControl> ExportList { get; set; }
        List<IRepositoryElement> RepositoryElementList { get; set; }       
        List<string> PrebuildCommandList { get; set; }
        Dictionary<string, object> ProjectInfo { get; set; }
        
    }
}