public interface ICommit : IChainLink<ICommit> {
    string GetMessage();
    void addObject(VersionController versionedObject, int version);
    int getObjectVersion(VersionController versionedObject);

}