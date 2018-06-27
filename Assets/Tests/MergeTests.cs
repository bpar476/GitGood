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
        ICommit secondCommit = versionManager.Commit("Move testObject");

        versionManager.Checkout("master");

        Assert.AreEqual("master", master.GetName());
        Assert.AreSame(commit, master.GetTip());
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreSame(secondCommit, testBranch.GetTip());

        IMergeWorker mw = new MergeWorker(master, testBranch);
        Assert.AreEqual(Relationship.FastForward, mw.GetMergeType());
        Assert.AreEqual(true, mw.IsFastForward(testController1));
        Assert.AreEqual(true, mw.IsFastForward(testController2));
        Assert.AreEqual(true, mw.IsResolved());
    }

    [TearDown]
    public void TearDown() {

    }
}