using Horn.Core.SCM;

namespace Horn.Core.PackageStructure
{
    public interface IRevisionData
    {
        GetOperation Operation();

        void RecordRevision(IPackageTree packageTree, string revisionVlaue);

        string Revision { get; }

        bool ShouldCheckOut();

        bool ShouldUpdate(IRevisionData other);
    }
}