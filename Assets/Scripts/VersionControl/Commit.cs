using System;
using System.Collections.Generic;

public class Commit : ICommit {

    private IDictionary<VersionController, int> objectData;
    private ICommit parent;
    private string commitMessage;


    public Commit(ICommit parent, string commitMessage) {
        Relink(parent);
        this.commitMessage = commitMessage;
        this.objectData = new Dictionary<VersionController, int>();
    }

    public string GetMessage() {
        return commitMessage;

    }

    public void Relink(ICommit parent) {
        this.parent = parent;
    }

    public ICommit GetParent() {
        return parent;
    }

    public void addObject(VersionController versionedObject, int version) {
        objectData.Add(versionedObject, version);
    }

    public int getObjectVersion(VersionController versionedObject) {
        int version;
        if (this.objectData.TryGetValue(versionedObject, out version)) {
            return version;
        }
        else {
            throw new System.ArgumentException("Key doesn't exist");
        }
    }
}