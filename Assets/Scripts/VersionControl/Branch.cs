public class Branch : IBranch {
    private ICommit branchTip;
    private string branchName;

    public Branch(string name, ICommit tip) {
        branchTip = tip;
        branchName = name;
    }

    public string GetName() {
        return branchName;
    }

    public ICommit GetTip() {
        return branchTip;
    }

    public void UpdateTip(ICommit commit) {
        this.branchTip = commit;
    }
}