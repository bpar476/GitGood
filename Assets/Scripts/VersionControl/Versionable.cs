using UnityEngine;

public interface Versionable {
    void Stage(GameObject version);

    void Commit(int version);

    void ResetToVersion(int version, GameObject target);

    void ResetToStaged(GameObject target);
}
