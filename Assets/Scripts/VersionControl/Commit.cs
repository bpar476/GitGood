using System;
using System.Collections.Generic;

/// <summary>
/// Commits represent the state of a level at a point in time. Contains versionable objects and their versions.
/// </summary>
public class Commit : ICommit {

    private IDictionary<VersionController, IVersion> objectData;
    private ICommit parent;
    private string commitMessage;
    private Guid id;

    /// <summary>
    /// The Commit constructor should not be called directly, instead use a CommitBuilder to incrementally build a commit
    /// </summary>
    public Commit(ICommit parent, IDictionary<VersionController, IVersion> versionData, string message) {
        Relink(parent);
        this.commitMessage = message;
        this.objectData = new Dictionary<VersionController, IVersion>();
        foreach (VersionController controller in versionData.Keys) {
            this.objectData.Add(controller, versionData[controller]);
        }
        this.id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets the commit message for this commit
    /// </summary>
    public string GetMessage() {
        return commitMessage;
    }

    /// <summary>
    /// Changes the parent commit of this parent
    /// </summary>
    public void Relink(ICommit parent) {
        this.parent = parent;
    }

    /// <summary>
    /// Gets the parent of this commit
    /// </summary>
    public ICommit GetParent() {
        return parent;
    }

    /// <summary>
    /// Gets the version code for the given object corresponding to the state at this commit
    /// </summary>
    public IVersion getObjectVersion(VersionController versionedObject) {
        IVersion version;
        if (this.objectData.TryGetValue(versionedObject, out version)) {
            return version;
        }
        else {
            throw new System.ArgumentException("Key doesn't exist");
        }
    }

    /// <summary>
    /// Gets the ID for this commit.
    /// </summary>
    public Guid GetCommitId() {
        return this.id;
    }

    /// <summary>
    /// Determines whether an object is tracked in this commit
    /// </summary>
    public bool ObjectIsTrackedInThisCommit(VersionController controller) {
        return this.objectData.ContainsKey(controller);
    }

    /// <summary>
    /// Returns an IEnumerator over all the objects that are tracked in this commit
    /// </summary>
    public IEnumerator<VersionController> GetTrackedObjectsEnumerator() {
        return this.objectData.Keys.GetEnumerator();
    }
    public ICollection<VersionController> GetTrackedObjects() {
        return objectData.Keys;
    }
}