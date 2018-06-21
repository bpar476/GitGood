using System.Collections.Generic;

/// <summary>
/// A builder class for instantiating commits. Allows for incremental addition of object state to the commit.
/// Allows commit state to be immutable.
/// </summary>
public class CommitBuilder {

    private IDictionary<VersionController, int> versionData;
    private ICommit parent;
    private string message;
    private ICommit buildTarget;

    /// <summary>
    /// Creates a fresh CommitBuilder
    /// </summary>
    public CommitBuilder() {
        this.versionData = new Dictionary<VersionController, int>();
    }

    /// <summary>
    /// Sets the parent of the commit to be instantiated. Returns this instance so methods can be chained.
    /// </summary>
    public CommitBuilder SetParent(ICommit parent) {
        this.parent = parent;
        return this;
    }

    /// <summary>
    /// Sets the commit message of the commit to be instantiated' Returns this instance so methods can be chained.
    /// </summary>
    public CommitBuilder SetMessage(string message) {
        this.message = message;
        return this;
    }

    /// <summary>
    /// Adds the given versionable object to the commit, with the state being the current version. Returns this instance so methods can be chained.
    /// </summary>
    public CommitBuilder AddObject(VersionController versionable, int version) {
        this.versionData.Add(versionable, version);
        return this;
    }

    /// <summary>
    /// Builds the commit that has been developed so far and returns it. Subsequent calls to build will return the original commit.
    /// </summary>
    public ICommit Build() {
        if (this.buildTarget == null) {
            this.buildTarget = new Commit(parent, versionData, message);
        }
        return this.buildTarget;
    }

}
