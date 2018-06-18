using UnityEngine;

public interface Versionable {
    void Stage(GameObject version);

    void Commit(int commitId);

    void ResetToCommit(int commitId, GameObject target);
}