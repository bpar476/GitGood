
public interface Versionable {
    void Stage();

    void Commit(int commitId);

    void ResetToCommit(int commitId);
}