using System;

public class MergeWorker : IMergeWorker
{
    private IBranch baseBranch, featureBranch;
    private VersionManager versionManager;
    private bool isMergable;

    public MergeWorker(VersionManager versionManager, IBranch baseBranch, IBranch featureBranch) {
        if (baseBranch == null || featureBranch == null) {
            throw new Exception("Branch can not be null");
        }
        this.baseBranch = baseBranch;
        this.featureBranch = featureBranch;
        this.isMergable = false;
        this.versionManager = versionManager;
        Initialise();
    }

    private void Initialise() {
        switch (LineageAnalyser.Compare(this.baseBranch.GetTip(), this.featureBranch.GetTip())) {
            case Relationship.Rewind:
                throw new Exception("Base branch is ahead of feature branch, merge redundant");
            case Relationship.Same:
                throw new Exception("Branches are the same");
            case Relationship.Unknown:
                throw new Exception("Can not determine branch relativity");
            case Relationship.FastForward:
                foreach (VersionController trackedObject in featureBranch.GetTip().GetTrackedObjects()) {
                    this.versionManager.Add(trackedObject);
                    this.isMergable = true;
                }
                break;
            case Relationship.Divergent:
                break;
            default:
                break;
        }
    }

    public void Abort() {
        throw new NotImplementedException();
    }

    public void End() {
        throw new NotImplementedException();
    }

    public bool IsResolved() {
        return this.isMergable;
    }

    public void PickVersion(VersionController vc, IVersion version) {
        throw new NotImplementedException();
    }

    public void RenderDiff() {
        throw new NotImplementedException();
    }
}