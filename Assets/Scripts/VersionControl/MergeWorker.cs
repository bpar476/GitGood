using System;

public class MergeWorker : IMergeWorker
{
    private IBranch baseBranch, featureBranch;
    private bool isMergable;

    public MergeWorker(IBranch baseBranch, IBranch featureBranch) {
        this.baseBranch = baseBranch;
        this.featureBranch = featureBranch;
        this.isMergable = false;
        Initialise();
    }

    private void Initialise() {

    }

    public void Abort()
    {
        throw new System.NotImplementedException();
    }

    public void End()
    {
        throw new System.NotImplementedException();
    }

    public bool IsResolved()
    {
        return this.isMergable;
    }

    public void PickVersion(VersionController vc, int version)
    {
        throw new System.NotImplementedException();
    }

    public void RenderDiff()
    {
        throw new System.NotImplementedException();
    }
}