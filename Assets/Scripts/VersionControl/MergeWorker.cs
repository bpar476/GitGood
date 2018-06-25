using System;

public class MergeWorker : IMergeWorker
{
    private IBranch baseBranch, featureBranch;
    private bool isMergable;

    public MergeWorker(IBranch baseBranch, IBranch featureBranch) {
        if (baseBranch == null || featureBranch == null) {
            throw new Exception("Branch can not be null");
        }
        this.baseBranch = baseBranch;
        this.featureBranch = featureBranch;
        this.isMergable = false;
        Initialise();
    }

    private void Initialise() {
        switch (BranchAnalyser.Compare(this.baseBranch, this.featureBranch)) {
            case BranchCompareAnalysis.Rewind:
                throw new Exception("Base branch is ahead of feature branch, merge redundant");
            case BranchCompareAnalysis.Same:
                throw new Exception("Branches are the same");
            case BranchCompareAnalysis.Unknown:
                throw new Exception("Can not determine branch relativity");
            case BranchCompareAnalysis.Divergent:
            case BranchCompareAnalysis.FastForward:
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

    public void PickVersion(VersionController vc, int version) {
        throw new NotImplementedException();
    }

    public void RenderDiff() {
        throw new NotImplementedException();
    }
}