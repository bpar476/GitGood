using System;

public class Version : IVersion {
    private Guid guid;
    private IVersion parent;
    public Version(IVersion parent) {
        this.guid = Guid.NewGuid();
        Relink(parent);
    }

    public Version() : this(null) {
    }

    public Guid GetId() {
        return this.guid;
    }

    public IVersion GetParent() {
        return this.parent;
    }

    public void Relink(IVersion parent) {
        this.parent = parent;
    }
}