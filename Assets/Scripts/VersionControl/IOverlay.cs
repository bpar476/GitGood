using UnityEngine;

public interface IOverlay {
    ICommit GetCommit();
    void Create();
    void Destroy();
    void RemoveObject(VersionController versionedObject);
    bool ContainsObject(VersionController versionedObject);
    void SetColor(Color color);
    void SetColor(VersionController versionedObject, Color color);
    void SetColor(GameObject gameObject, Color color);
    void EnableCollision(VersionController versionedObject);
    void DisableCollision(VersionController versionedObject);
    bool HasGameObject(GameObject gameObject, out VersionController versionedObject);
}