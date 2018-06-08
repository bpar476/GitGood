
public interface Versionable {
    void Stage();

    void Commit(int commitId);

    void resetToCommit(int commitId);
}