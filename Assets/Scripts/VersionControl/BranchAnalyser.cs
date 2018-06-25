using System.Collections.Generic;

public class BranchAnalyser {
    public static BranchCompareAnalysis Compare(IBranch baseBranch, IBranch featureBranch) {
        if (baseBranch.GetTip() == featureBranch.GetTip()) {
            return BranchCompareAnalysis.Same;
        }

        HashSet<ICommit> baseCommits = new HashSet<ICommit>();
        HashSet<ICommit> featureCommits = new HashSet<ICommit>();

        ICommit commit = featureBranch.GetTip();
        while (commit != null) {
            if (commit == baseBranch.GetTip()) {
                return BranchCompareAnalysis.FastForward;
            }
            featureCommits.Add(commit);
            commit = commit.GetParent();
        }

        commit = baseBranch.GetTip();
        while (commit != null) {
            if (commit == featureBranch.GetTip()) {
                return BranchCompareAnalysis.Rewind;
            }
            baseCommits.Add(commit);
            commit = commit.GetParent();
        }

        commit = featureBranch.GetTip();
        while (commit != null) {
            if (baseCommits.Contains(commit)) {
                return BranchCompareAnalysis.Divergent;
            }
            commit = commit.GetParent();
        }

        return BranchCompareAnalysis.Unknown;
    }
}