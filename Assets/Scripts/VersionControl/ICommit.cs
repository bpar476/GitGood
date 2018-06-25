using System;
using System.Collections.Generic;

public interface ICommit : IChainLink<ICommit> {
    string GetMessage();
    int getObjectVersion(VersionController versionedObject);
    Guid GetCommitId();
    bool ObjectIsTrackedInThisCommit(VersionController controller);
}
