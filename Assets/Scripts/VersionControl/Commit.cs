public class Commit : ICommit {

    private ICommit parent;
    private string commitMessage;

    public Commit(ICommit parent, string commitMessage) {
        Relink(parent);
        this.commitMessage = commitMessage;
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

}