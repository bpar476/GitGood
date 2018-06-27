using System;
using System.Collections.Generic;

public interface ICommit : IChainLink<ICommit>, IIdentifiable {
    string GetMessage();
    IVersion getObjectVersion(VersionController versionedObject);
    bool ObjectIsTrackedInThisCommit(VersionController controller);
    IEnumerator<VersionController> GetTrackedObjectsEnumerator();
    ICollection<VersionController> GetTrackedObjects();
}
