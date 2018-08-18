using System.Collections.Generic;
using UnityEngine;

public interface IMergeWorker {
    bool IsResolved();
    void Abort();
    void End();
    void PickVersion(VersionController versionedObject, IVersion version);
    void RenderDiff();
    IDictionary<VersionController, IVersion> BuildStagingArea();
    MergeStatus GetStatus(VersionController versionedObject);
    Relationship GetMergeType();
    void PickObject(GameObject gameObject);
    GameObject GetBasePreviewForVersionedObject(VersionController versionedObject);
    GameObject GetFeaturePreviewForVersionedObject(VersionController versionedObject);
}