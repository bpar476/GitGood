public interface IMergeWorker {
    bool IsResolved();
    void Abort();
    void End();
    void PickVersion(VersionController vc, int version);
    void RenderDiff();
}