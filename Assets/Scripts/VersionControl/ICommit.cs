using System;

public interface ICommit : IChainLink<ICommit> {
    string GetMessage();
    int getObjectVersion(VersionController versionedObject);
    Guid GetCommitId();
}
