using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MergeTests {
    VersionableObjectFactory factory = new VersionableObjectFactory();

    // VersionManager versionManager;
    VersionController testController1, testController2;
    GameObject testObject1, testObject2;
    IBranch master;

    [SetUp]
    public void SetUp() {
        VersionManager.Reset();

        testController1 = factory.createVersionablePlayer();
        testController2 = factory.createVersionableBox();

        testObject1 = testController1.GetActiveVersion();
        testObject2 = testController2.GetActiveVersion();

        master = VersionManager.Instance().GetActiveBranch();
    }

    [UnityTest]
    public IEnumerator TestMergeWorkerSmoke() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController1);
        VersionManager.Instance().Add(testController2);

        ICommit commit = VersionManager.Instance().Commit("Add two objects");

        yield return null;

        IBranch testBranch = VersionManager.Instance().CreateBranch("testBranch");
        VersionManager.Instance().Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        VersionManager.Instance().Add(testController1);
        ICommit secondCommit = VersionManager.Instance().Commit("Move testObject1");

        VersionManager.Instance().Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        IMergeWorker mw = new MergeWorker(master, testBranch, null, null);
        Assert.AreEqual(Relationship.FastForward, mw.GetMergeType());
        Assert.AreEqual(MergeStatus.FastForward, mw.GetStatus(testController1));
        Assert.AreEqual(MergeStatus.FastForward, mw.GetStatus(testController2));
        Assert.AreEqual(true, mw.IsResolved());
    }

    [UnityTest]
    public IEnumerator TestMergeFF() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController1);
        VersionManager.Instance().Add(testController2);

        ICommit commit = VersionManager.Instance().Commit("Add two objects");

        yield return null;

        IBranch testBranch = VersionManager.Instance().CreateBranch("testBranch");
        VersionManager.Instance().Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        VersionManager.Instance().Add(testController1);
        ICommit secondCommit = VersionManager.Instance().Commit("Move testObject1");

        VersionManager.Instance().Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = VersionManager.Instance().Merge(testBranch);
        Assert.AreEqual(Relationship.FastForward, mergeType);

        Assert.AreSame(secondCommit, master.GetTip());
        Assert.AreSame(secondCommit, VersionManager.Instance().GetActiveCommit());

    }

    [UnityTest]
    public IEnumerator TestMergeDD() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController1);
        VersionManager.Instance().Add(testController2);

        VersionManager.Instance().Commit("Add two objects");

        yield return null;

        IBranch testBranch = VersionManager.Instance().CreateBranch("testBranch");
        VersionManager.Instance().Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        VersionManager.Instance().Add(testController1);
        ICommit secondCommit = VersionManager.Instance().Commit("Move testObject1");

        VersionManager.Instance().Checkout("master");

        testObject2.transform.position = new Vector2(6.0f, 6.0f);
        VersionManager.Instance().Add(testController2);
        ICommit thirdCommit = VersionManager.Instance().Commit("Move testObject2");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(thirdCommit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = VersionManager.Instance().Merge(testBranch);
        Assert.AreEqual(Relationship.Divergent, mergeType);
        Assert.AreEqual(MergeStatus.FastForward, VersionManager.Instance().GetMergeWorker().GetStatus(testController1));
        Assert.AreEqual(MergeStatus.FastForward, VersionManager.Instance().GetMergeWorker().GetStatus(testController2));

        Assert.AreEqual(false, VersionManager.Instance().IsInMergeConflict());
        ICommit mergeCommit = VersionManager.Instance().ResolveMerge();
        Assert.NotNull(mergeCommit);

        Assert.AreSame(mergeCommit, master.GetTip());
        Assert.AreSame(mergeCommit, VersionManager.Instance().GetActiveCommit());

    }

    [UnityTest]
    public IEnumerator TestMergeDivergent() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController1);
        VersionManager.Instance().Add(testController2);

        VersionManager.Instance().Commit("Add two objects");

        yield return null;

        IBranch testBranch = VersionManager.Instance().CreateBranch("testBranch");
        VersionManager.Instance().Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        testObject2.transform.position = new Vector2(0.0f, 0.0f);
        VersionManager.Instance().Add(testController1);
        VersionManager.Instance().Add(testController2);
        ICommit secondCommit = VersionManager.Instance().Commit("Move testObject1 and testObject2");

        VersionManager.Instance().Checkout("master");

        testObject2.transform.position = new Vector2(6.0f, 6.0f);
        VersionManager.Instance().Add(testController2);
        ICommit thirdCommit = VersionManager.Instance().Commit("Move testObject2");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(thirdCommit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = VersionManager.Instance().Merge(testBranch);
        Assert.AreEqual(Relationship.Divergent, mergeType);
        Assert.AreEqual(MergeStatus.FastForward, VersionManager.Instance().GetMergeWorker().GetStatus(testController1));
        Assert.AreEqual(MergeStatus.Conflict, VersionManager.Instance().GetMergeWorker().GetStatus(testController2));
        Assert.AreEqual(true, VersionManager.Instance().IsInMergeConflict());

        VersionManager.Instance().GetMergeWorker().PickVersion(testController2, master.GetTip().getObjectVersion(testController2));

        Assert.AreEqual(MergeStatus.Resolved, VersionManager.Instance().GetMergeWorker().GetStatus(testController2));
        Assert.AreEqual(false, VersionManager.Instance().IsInMergeConflict());

        ICommit mergeCommit = VersionManager.Instance().ResolveMerge();
        Assert.NotNull(mergeCommit);

        Assert.AreSame(mergeCommit, master.GetTip());
        Assert.AreSame(mergeCommit, VersionManager.Instance().GetActiveCommit());
    }

    [TearDown]
    public void TearDown() {

    }
}
