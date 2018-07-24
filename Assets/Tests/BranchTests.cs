using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BranchTests {
    [SetUp]
    public void SetUp() {
        VersionManager.Reset();
    }

    [UnityTest]
    public IEnumerator TestSimple() {
        ICommit a = new CommitBuilder().SetMessage("Commit A").Build();
        ICommit b = new CommitBuilder().SetParent(a).SetMessage("Commit B").Build();

        yield return null;
        Assert.AreEqual("Commit A", b.GetParent().GetMessage());
    }

    [UnityTest]
    public IEnumerator TestBranchConstruction() {
        ICommit A = new CommitBuilder().SetMessage("Test commit object construction").Build();
        IBranch testBranch = new Branch("feature/branches", A);
        yield return null;
        Assert.AreSame(A, testBranch.GetTip());
        Assert.AreEqual("feature/branches", testBranch.GetName());

    }

    [UnityTest]
    public IEnumerator TestSwitchBranch() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = VersionManager.Instance();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        ICommit commit = VersionManager.Instance().Commit("Add two objects");

        yield return null;
        Assert.AreSame(commit, VersionManager.Instance().GetActiveBranch().GetTip());

        IBranch testBranch = VersionManager.Instance().CreateBranch("testBranch");
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreEqual(true, VersionManager.Instance().Checkout("testBranch"));
        Assert.AreSame(testBranch, VersionManager.Instance().GetActiveBranch());
        Assert.AreSame(commit, VersionManager.Instance().GetActiveBranch().GetTip());

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        VersionManager.Instance().Add(testController);
        ICommit secondCommit = VersionManager.Instance().Commit("Move testObject");

        Assert.AreSame(secondCommit, VersionManager.Instance().GetActiveBranch().GetTip());
        Assert.AreSame(secondCommit, VersionManager.Instance().GetHead());
        Assert.AreSame(commit, VersionManager.Instance().LookupBranch("master").GetTip());

        VersionManager.Instance().Checkout("master");
        Assert.AreSame(commit, VersionManager.Instance().GetHead());


    }

    [UnityTest]
    public IEnumerator TestFastForward() {
        VersionManager versionManager = VersionManager.Instance();
        VersionManager.Instance().Commit("Initial Commit");
        IBranch master = VersionManager.Instance().GetActiveBranch();
        IBranch feature = VersionManager.Instance().CreateBranch("feature");

        VersionManager.Instance().Checkout(feature.GetName());
        VersionManager.Instance().Commit("Commit on feature branch");
        yield return null;

        Assert.AreEqual(Relationship.FastForward, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestRewind() {
        VersionManager versionManager = VersionManager.Instance();
        VersionManager.Instance().Commit("Initial Commit");
        IBranch master = VersionManager.Instance().GetActiveBranch();
        IBranch feature = VersionManager.Instance().CreateBranch("feature");

        VersionManager.Instance().Commit("Commit on master branch");
        yield return null;

        Assert.AreEqual(Relationship.Rewind, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestSame() {
        VersionManager versionManager = VersionManager.Instance();
        VersionManager.Instance().Commit("Initial Commit");
        IBranch master = VersionManager.Instance().GetActiveBranch();
        IBranch feature = VersionManager.Instance().CreateBranch("feature");

        yield return null;

        Assert.AreEqual(Relationship.Same, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestDivergent() {
        VersionManager versionManager = VersionManager.Instance();
        VersionManager.Instance().Commit("Initial Commit");
        IBranch master = VersionManager.Instance().GetActiveBranch();
        IBranch feature = VersionManager.Instance().CreateBranch("feature");

        VersionManager.Instance().Commit("Commit on master branch");
        VersionManager.Instance().Checkout(feature.GetName());
        VersionManager.Instance().Commit("Commit on feature branch");
        yield return null;

        Assert.AreEqual(Relationship.Divergent, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }
}
