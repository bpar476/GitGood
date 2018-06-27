using System.Collections.Generic;

public interface IMergeWorker {
    bool IsResolved();
    void Abort();
    void End();
    void PickVersion(VersionController versionedObject, IVersion version);
    void RenderDiff();
    IDictionary<VersionController, IVersion> BuildStagingArea();
    MergeStatus GetStatus(VersionController versionedObject);
    Relationship GetMergeType();
}