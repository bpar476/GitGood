using System.Collections.Generic;

public interface IMergeWorker {
    bool IsResolved();
    void Abort();
    void End();
    void PickVersion(VersionController versionedObject, IVersion version);
    void RenderDiff();
    IDictionary<VersionController, IVersion> BuildStagingArea();
    bool IsConflict(VersionController versionedObject);
    bool IsResolved(VersionController versionedObject);
    bool IsFastForward(VersionController versionedObject);
    Relationship GetMergeType();
}