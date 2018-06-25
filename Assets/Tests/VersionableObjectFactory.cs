using UnityEngine;

public class VersionableObjectFactory {
    private VersionController createTransformVersionedObject(string prefabName) {
        VersionController testObject = new GameObject().AddComponent<VersionController>();
        TransformVersionable transformVersioner = testObject.gameObject.AddComponent<TransformVersionable>();
        testObject.AddVersionable(transformVersioner);
        testObject.SetActiveVersion(new GameObject());
        testObject.SetTemplatePrefab(Resources.Load("Tests/Versionables/" + prefabName) as GameObject);
        testObject.SetPreviewPrefab(Resources.Load("Tests/Versionables/" + prefabName + "Preview") as GameObject);
        return testObject;
    }

    public VersionController createVersionablePlayer() {
        return createTransformVersionedObject("Player");
    }

    public VersionController createVersionableBox() {
        return createTransformVersionedObject("VersionableBox");
    }
}