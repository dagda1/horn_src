using System;
using System.Collections.Generic;
using Horn.Core.Dsl;
using Horn.Core.SCM;

namespace Horn.Core.Spec.Doubles
{
    public class BuildMetaDataStub : IBuildMetaData
    {
        public BuildEngines.BuildEngine BuildEngine { get; set; }

        public string Description { get; set; }

        public List<SourceControl> ExportList { get; set; }

        public string InstallName { get; set; }

        public List<string> PrebuildCommandList { get; set; }

        public Dictionary<string, object> ProjectInfo { get; set; }

        public List<IRepositoryElement> RepositoryElementList { get; set; }

        public SourceControl SourceControl { get; set; }

        public BuildMetaDataStub(BuildEngines.BuildEngine buildEngine, SourceControl sourceControl)
        {
            BuildEngine = buildEngine;
            SourceControl = sourceControl;
            PrebuildCommandList = new List<string>();
            RepositoryElementList = new List<IRepositoryElement>();
            ExportList = new List<SourceControl>();
        }
    }
}