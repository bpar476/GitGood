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

    /// <summary>
    /// Determine whether this overlay contains a versioned object representation
    /// </summary>
    public bool ContainsObject(VersionController versionedObject) {
        return this.overlayObjects.ContainsKey(versionedObject);
    }

    /// <summary>
    /// Create the overlay, given the state of the constructed Overlay
    /// </summary>
    public void Create() {
        foreach(VersionController versionedOject in this.commit.GetTrackedObjects()) {
            GameObject constructedObject = versionedOject.ReconstructVersion(this.commit.getObjectVersion(versionedOject));
            this.overlayObjects.Add(versionedOject, constructedObject);
        }
    }

    /// <summary>
    /// Destroy the overlay. This will remove it from view. The overlay can be recreated through Create()
    /// </summary>
    public void Destroy() {
        foreach (VersionController versionedObject in overlayObjects.Keys) {
            Object.Destroy(this.overlayObjects[versionedObject]);
        }
        this.overlayObjects.Clear();
    }

    /// <summary>
    /// Get the commit that this Overlay is displaying
    /// </summary>
    public ICommit GetCommit() {
        return this.commit;
    }

    /// <summary>
    /// Remove an object from the overlay
    /// </summary>
    public void RemoveObject(VersionController versionedObject) {
        Object.Destroy(this.overlayObjects[versionedObject]);
        this.overlayObjects.Remove(versionedObject);
    }

    /// <summary>
    /// Set the colour of the objects in the overlay
    /// </summary>
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