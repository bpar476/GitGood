public interface IBranch {
    ICommit GetTip();
    void UpdateTip(ICommit commit);
    string GetName();
}