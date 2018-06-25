using System.Collections.Generic;

public class History<T> {

    private IDictionary<IVersion, T> versions;

    public History() {
        versions = new Dictionary<IVersion, T>();
    }

    public void Add(IVersion version, T state) {
        versions.Add(version, state);
    }

    public T GetStateAt(IVersion version) {
        T result;
        if(versions.TryGetValue(version, out result)) {
            return result;
        } else {
            throw new System.ArgumentException("Commit id does not exist in this history");
        }
    }
}