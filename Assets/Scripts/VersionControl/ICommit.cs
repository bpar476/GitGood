public interface ICommit : IChainLink<ICommit> {
    string GetMessage();
    int getObjectVersion(VersionController versionedObject);
}
