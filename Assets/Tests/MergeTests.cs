using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MergeTests {
    VersionableObjectFactory factory = new VersionableObjectFactory();

    VersionManager versionManager;
    VersionController testController1, testController2;
    GameObject testObject1, testObject2;
    IBranch master;

    [SetUp]
    public void SetUp() {
        versionManager = new GameObject().AddComponent<VersionManager>();

        testController1 = factory.createVersionablePlayer();
        testController2 = factory.createVersionableBox();

        testObject1 = testController1.GetActiveVersion();
        testObject2 = testController2.GetActiveVersion();

        master = versionManager.GetActiveBranch();
    }

    [UnityTest]
    public IEnumerator TestMergeWorkerSmoke() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController1);
        versionManager.Add(testController2);

        ICommit commit = versionManager.Commit("Add two objects");

        yield return null;

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        versionManager.Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testController1);
        ICommit secondCommit = versionManager.Commit("Move testObject1");

        versionManager.Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        IMergeWorker mw = new MergeWorker(master, testBranch);
        Assert.AreEqual(Relationship.FastForward, mw.GetMergeType());
        Assert.AreEqual(MergeStatus.FastForward, mw.GetStatus(testController1));
        Assert.AreEqual(MergeStatus.FastForward, mw.GetStatus(testController2));
        Assert.AreEqual(true, mw.IsResolved());
    }

    [UnityTest]
    public IEnumerator TestMergeFF() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController1);
        versionManager.Add(testController2);

        ICommit commit = versionManager.Commit("Add two objects");

        yield return null;

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        versionManager.Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testController1);
        ICommit secondCommit = versionManager.Commit("Move testObject1");

        versionManager.Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = versionManager.Merge(testBranch);
        Assert.AreEqual(Relationship.FastForward, mergeType);

        Assert.AreSame(secondCommit, master.GetTip());
        Assert.AreSame(secondCommit, versionManager.GetActiveCommit());

    }

    [UnityTest]
    public IEnumerator TestMergeDD() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController1);
        versionManager.Add(testController2);

        versionManager.Commit("Add two objects");

        yield return null;

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        versionManager.Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testController1);
        ICommit secondCommit = versionManager.Commit("Move testObject1");

        versionManager.Checkout("master");

        testObject2.transform.position = new Vector2(6.0f, 6.0f);
        versionManager.Add(testController2);
        ICommit thirdCommit = versionManager.Commit("Move testObject2");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(thirdCommit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = versionManager.Merge(testBranch);
        Assert.AreEqual(Relationship.Divergent, mergeType);
        Assert.AreEqual(MergeStatus.FastForward, versionManager.GetMergeWorker().GetStatus(testController1));
        Assert.AreEqual(MergeStatus.FastForward, versionManager.GetMergeWorker().GetStatus(testController2));

        Assert.AreEqual(false, versionManager.IsInMergeConflict());
        ICommit mergeCommit = versionManager.ResolveMerge();
        Assert.NotNull(mergeCommit);

        Assert.AreSame(mergeCommit, master.GetTip());
        Assert.AreSame(mergeCommit, versionManager.GetActiveCommit());

    }

    [UnityTest]
    public IEnumerator TestMergeDivergent() {
        testObject1.transform.position = new Vector2(0.0f, 0.0f);
        testObject2.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testController1);
        versionManager.Add(testController2);

        versionManager.Commit("Add two objects");

        yield return null;

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        versionManager.Checkout("testBranch");

        testObject1.transform.position = new Vector2(1.0f, 0.0f);
        testObject2.transform.position = new Vector2(0.0f, 0.0f);
        versionManager.Add(testController1);
        versionManager.Add(testController2);
        ICommit secondCommit = versionManager.Commit("Move testObject1 and testObject2");

        versionManager.Checkout("master");

        testObject2.transform.position = new Vector2(6.0f, 6.0f);
        versionManager.Add(testController2);
        ICommit thirdCommit = versionManager.Commit("Move testObject2");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(thirdCommit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        Relationship mergeType = versionManager.Merge(testBranch);
        Assert.AreEqual(Relationship.Divergent, mergeType);
        Assert.AreEqual(MergeStatus.FastForward, versionManager.GetMergeWorker().GetStatus(testController1));
        Assert.AreEqual(MergeStatus.Conflict, versionManager.GetMergeWorker().GetStatus(testController2));
        Assert.AreEqual(true, versionManager.IsInMergeConflict());

        versionManager.GetMergeWorker().PickVersion(testController2, master.GetTip().getObjectVersion(testController2));

        Assert.AreEqual(MergeStatus.Resolved, versionManager.GetMergeWorker().GetStatus(testController2));
        Assert.AreEqual(false, versionManager.IsInMergeConflict());

        ICommit mergeCommit = versionManager.ResolveMerge();
        Assert.NotNull(mergeCommit);

        Assert.AreSame(mergeCommit, master.GetTip());
        Assert.AreSame(mergeCommit, versionManager.GetActiveCommit());
    }

    [TearDown]
    public void TearDown() {

    }
}