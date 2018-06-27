using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BranchTests {
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

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        ICommit commit = versionManager.Commit("Add two objects");

        yield return null;
        Assert.AreSame(commit, versionManager.GetActiveBranch().GetTip());

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreEqual(true, versionManager.Checkout("testBranch"));
        Assert.AreSame(testBranch, versionManager.GetActiveBranch());
        Assert.AreSame(commit, versionManager.GetActiveBranch().GetTip());

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testController);
        ICommit secondCommit = versionManager.Commit("Move testObject");

        Assert.AreSame(secondCommit, versionManager.GetActiveBranch().GetTip());
        Assert.AreSame(secondCommit, versionManager.GetHead());
        Assert.AreSame(commit, versionManager.LookupBranch("master").GetTip());

        versionManager.Checkout("master");
        Assert.AreSame(commit, versionManager.GetHead());


    }

    [UnityTest]
    public IEnumerator TestFastForward() {
        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();
        versionManager.Commit("Initial Commit");
        IBranch master = versionManager.GetActiveBranch();
        IBranch feature = versionManager.CreateBranch("feature");

        versionManager.Checkout(feature.GetName());
        versionManager.Commit("Commit on feature branch");
        yield return null;

        Assert.AreEqual(Relationship.FastForward, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestRewind() {
        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();
        versionManager.Commit("Initial Commit");
        IBranch master = versionManager.GetActiveBranch();
        IBranch feature = versionManager.CreateBranch("feature");

        versionManager.Commit("Commit on master branch");
        yield return null;

        Assert.AreEqual(Relationship.Rewind, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestSame() {
        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();
        versionManager.Commit("Initial Commit");
        IBranch master = versionManager.GetActiveBranch();
        IBranch feature = versionManager.CreateBranch("feature");

        yield return null;

        Assert.AreEqual(Relationship.Same, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestDivergent() {
        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();
        versionManager.Commit("Initial Commit");
        IBranch master = versionManager.GetActiveBranch();
        IBranch feature = versionManager.CreateBranch("feature");

        versionManager.Commit("Commit on master branch");
        versionManager.Checkout(feature.GetName());
        versionManager.Commit("Commit on feature branch");
        yield return null;

        Assert.AreEqual(Relationship.Divergent, LineageAnalyser.Compare(master.GetTip(), feature.GetTip()));
    }

    [UnityTest]
    public IEnumerator TestMergeWorkerSmoke() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();
        IBranch master = versionManager.GetActiveBranch();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        ICommit commit = versionManager.Commit("Add two objects");

        yield return null;

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        versionManager.Checkout("testBranch");

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testController);
        ICommit secondCommit = versionManager.Commit("Move testObject");

        versionManager.Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreEqual(commit, master.GetTip());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreEqual(secondCommit, testBranch.GetTip());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Assert.AreSame(commit, secondCommit.GetParent());
        Assert.AreEqual(commit, secondCommit.GetParent());

        Relationship r = LineageAnalyser.Compare(master.GetTip(), testBranch.GetTip());
        Assert.AreEqual(Relationship.FastForward, r);

        IMergeWorker mw = new MergeWorker(master, testBranch);
        Assert.AreEqual(true, mw.IsResolved());


    }
}
