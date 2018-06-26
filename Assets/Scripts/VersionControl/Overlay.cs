using System.Collections.Generic;
using UnityEngine;

public class Overlay : IOverlay
{
    private IDictionary<VersionController, GameObject> overlayObjects;
    private ICommit commit;

    public Overlay(ICommit commit) {
        this.commit = commit;
        this.overlayObjects = new Dictionary<VersionController, GameObject>();

        if (this.commit != null) {
            this.Create();
        }
    }

    public Overlay(ICommit commit, Color color) : this(commit) {
        this.SetColor(color);
    }

    public bool ContainsObject(VersionController versionedObject) {
        return this.overlayObjects.ContainsKey(versionedObject);
    }

    public void Create() {
        foreach(VersionController versionedOject in this.commit.GetTrackedObjects()) {
            GameObject constructedObject = versionedOject.ReconstructVersion(this.commit.getObjectVersion(versionedOject));
            this.overlayObjects.Add(versionedOject, constructedObject);
        }
    }

    public void Destroy() {
        foreach (VersionController versionedObject in overlayObjects.Keys) {
            Object.Destroy(this.overlayObjects[versionedObject]);
        }
        this.overlayObjects.Clear();
    }

    public ICommit GetCommit() {
        return this.commit;
    }

    public void RemoveObject(VersionController versionedObject) {
        Object.Destroy(this.overlayObjects[versionedObject]);
        this.overlayObjects.Remove(versionedObject);
    }

    public void SetColor(Color color) {
        foreach (VersionController versionedObject in overlayObjects.Keys) {
            this.SetColor(versionedObject, color);
        }
    }

    public void SetColor(VersionController versionedObject, Color color) {
        this.SetColor(this.overlayObjects[versionedObject], color);
    }

    public void SetColor(GameObject gameObject, Color color) {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = color;
    }
}