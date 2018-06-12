using System.Collections.Generic;

public class History<T> {

    private IDictionary<int, T> commits;

    public History() {
        commits = new Dictionary<int, T>();
    }

    public void Add(int commitId, T state) {
        commits.Add(commitId, state);
    }

    public T Load(int commitId) {
        T result;
        while(!commits.ContainsKey(commitId) && commitId >= 0) {
            commitId--;
        }
        if(commits.TryGetValue(commitId, out result)) {
            return result;
        } else {
            throw new System.ArgumentException("Commit id does not exist in this history");
        }
    }

    public void ResetOntoCommit(int commitId) {
        commitId++;
        while(commits.ContainsKey(commitId)) {
            commits.Remove(commitId);
            commitId++;
        }
    }
}