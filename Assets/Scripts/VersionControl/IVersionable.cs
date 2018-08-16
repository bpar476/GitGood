using UnityEngine;

public interface IVersionable {
    void Stage(GameObject version);

    void Commit(IVersion version);

    void ResetToVersion(IVersion version, GameObject target);

    void ResetToStaged(GameObject target);

    void ResetToInitialState(GameObject target);

    string DescribeState(IVersion version);

    string DescribeStagedState();
}
