using System;
using Horn.Core.SCM;

namespace Horn.Core.Dsl
{
    public class ExportData
    {
        public SourceControl SourceControl { get; private set; }

        private void SetSourceControl(string url, string sourceControlType, string path)
        {
            var scmType = sourceControlType.ToLower();

            if (scmType != "svn")
                throw new ArgumentOutOfRangeException(string.Format("Unkown SourceControlType {0}",
                                                                        sourceControlType));
            SourceControl = new SVNSourceControl(url, path);
        }

        public ExportData(string url, string sourceControlType)
        {
            SetSourceControl(url, sourceControlType, null);
        }

        public ExportData(string url, string sourceControlType, string path)
        {
            SetSourceControl(url, sourceControlType, path);
        }
    }
}