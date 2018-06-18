using System.Collections.Generic;

public class History<T> {

    private IDictionary<int, T> versions;

    public History() {
        versions = new Dictionary<int, T>();
    }

    public void Add(int version, T state) {
        versions.Add(version, state);
    }

    public T GetStateAt(int version) {
        T result;
        if(versions.TryGetValue(version, out result)) {
            return result;
        } else {
            throw new System.ArgumentException("Commit id does not exist in this history");
        }
    }

    public void ResetOntoVersion(int version) {
        version++;
        while(versions.ContainsKey(version)) {
            versions.Remove(version);
            version++;
        }
    }
}