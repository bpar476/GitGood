public class Branch : IBranch {
    private ICommit branchTip;

    public Branch() {
    }

    public ICommit GetTip() {
        return branchTip;
    }

    public void UpdateTip(ICommit commit) {
        this.branchTip = commit;
    }
}