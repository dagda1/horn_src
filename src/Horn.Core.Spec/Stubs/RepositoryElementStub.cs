using System.IO;
using Horn.Core.Dsl;

namespace Horn.Core.Spec.Doubles
{
    public class RepositoryElementStub : RepositoryElement
    {
        private bool _elementCopied;

        public bool ElementCopied
        {
            get { return _elementCopied; }
        }

        protected override void CopyElement(FileSystemInfo source, FileSystemInfo destination)
        {
            _elementCopied = true;
        }

        public RepositoryElementStub(string repositoryName, string includePath, string exportPath) : base(repositoryName, includePath, exportPath)
        {
        }
    }
}