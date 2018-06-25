using System;
using System.Collections.Generic;

public interface ICommit : IChainLink<ICommit> {
    string GetMessage();
    IVersion getObjectVersion(VersionController versionedObject);
    Guid GetCommitId();
    bool ObjectIsTrackedInThisCommit(VersionController controller);
    IEnumerator<VersionController> GetTrackedObjectsEnumerator();
    ICollection<VersionController> GetTrackedObjects();
}
