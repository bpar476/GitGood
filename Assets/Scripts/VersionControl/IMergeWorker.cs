public interface IMergeWorker {
    bool IsResolved();
    void Abort();
    void End();
    void PickVersion(VersionController vc, IVersion version);
    void RenderDiff();
}