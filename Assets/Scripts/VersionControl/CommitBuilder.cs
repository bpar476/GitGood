using System.Collections.Generic;

public class CommitBuilder {

    private IDictionary<VersionController, int> versionData;
    private ICommit parent;
    private string message;

    public CommitBuilder() {
        this.versionData = new Dictionary<VersionController, int>();
    }

    public void SetParent(ICommit parent) {
        this.parent = parent;
    }

    public void SetMessage(string message) {
        this.message = message;
    }

    public void AddObject(VersionController versionable, int version) {
        this.versionData.Add(versionable, version);
    }

    public ICommit build() {
        return new Commit(parent, versionData, message);
    }

}