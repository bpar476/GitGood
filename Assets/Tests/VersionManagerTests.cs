using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class VersionManagerTests {

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestResetPositionToHead() {
        // Given
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();

        GameObject testObject = testController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0,0);

        versionManager.Add(testController);
        versionManager.Commit("Set thing position to 0,0");

        yield return null;

        // When
        testObject.transform.position = new Vector2(1,0);
        versionManager.ResetToHead(testController);

        yield return null;

        // Then
        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetPositionBeforeHEAD() {
        // Given
        VersionableObjectFactory factory = new VersionableObjectFactory();
        
        VersionController testController = factory.createVersionablePlayer();

        GameObject testObject = testController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f,0.0f);

        versionManager.Add(testController);
        ICommit commitToLoad = versionManager.Commit("Set thing position to 0,0");

        yield return null;
        
        testObject.transform.position = new Vector2(1.0f,1.0f);

        versionManager.Add(testController);
        versionManager.Commit("Set thing position to 1,1");

        yield return null;
        
        // When
        versionManager.CheckoutCommit(commitToLoad);

        yield return null;

        // Then
        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetMultipleObjectPositions() {
        // Given
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        yield return null;

        versionManager.ResetToHead();

        yield return null;

        Assert.AreEqual(1.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(3.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(-1.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(5.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetOnePositionObjectToHeadButKeepOtherOneChanges() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(0.0f, 0.0f);

        yield return null;

        versionManager.ResetToHead(testController);

        yield return null;

        Assert.AreEqual(1.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(3.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(0.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetObjectsToCommitWhereOneWasNotChanged() {
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

        versionManager.Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(-2.0f, -2.0f);

        versionManager.Add(testController);
        versionManager.Commit("Move one of the objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(6.0f, 0.0f);

        yield return null;

        versionManager.ResetToHead();

        Assert.AreEqual(-2.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(-2.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(3.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(3.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestCheckoutCommitOnCurrentBranch() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        Guid firstCommitId = versionManager.Commit("Create two boxes").GetCommitId();

        yield return null;

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Move boxes");

        yield return null;

        versionManager.Checkout(versionManager.GetActiveBranch(), firstCommitId);

        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(3.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestCheckoutCommitOnDifferentBranch() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        Guid firstCommitId = versionManager.Commit("Create two boxes").GetCommitId();

        yield return null;

        versionManager.CreateBranch("feature");
        versionManager.CheckoutBranch("feature");

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Move boxes");

        yield return null;

        versionManager.Checkout("master", firstCommitId);

        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(3.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestObject.transform.position.y, 0.1f);

        IBranch master = versionManager.LookupBranch("master");
        Assert.AreEqual(master, versionManager.GetActiveBranch());
    }

    [UnityTest]
    public IEnumerator shouldResetObjectToInitialPositionIfCheckingOutCommitWhereItWasNotTracked() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        VersionController testController = factory.createVersionableBox();
        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        versionManager.Add(testController);

        Guid firstCommitId = versionManager.Commit("Create a box").GetCommitId();

        yield return null;

        VersionController otherTestController = factory.createVersionableBox();
        GameObject otherTestObject = otherTestController.GetActiveVersion();
        
        otherTestController.GetComponent<TransformVersionable>().SetInitialState(new Vector2(5.0f, 0.0f));

        testObject.transform.position = new Vector2(-3.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, -2.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Create another box and move the first box");

        yield return null;

        versionManager.Checkout(versionManager.GetActiveBranch(), firstCommitId);

        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(5.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestController.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator shouldResetTrackedObjectsWhenCheckingOutCommitWhereAnObjectWasNotTracked() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        VersionController testController = factory.createVersionableBox();
        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        versionManager.Add(testController);

        Guid firstCommitId = versionManager.Commit("Create a box").GetCommitId();

        yield return null;

        VersionController otherTestController = factory.createVersionableBox();
        GameObject otherTestObject = otherTestController.GetActiveVersion();
        
        testObject.transform.position = new Vector2(-3.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, -2.0f);

        versionManager.Add(testController);
        versionManager.Add(otherTestController);

        versionManager.Commit("Create another box and move the first box");

        yield return null;

        versionManager.Checkout(versionManager.GetActiveBranch(), firstCommitId);

        Assert.False(versionManager.IsObjectTracked(otherTestController));
    }

    [UnityTest]
    public IEnumerator shouldNotBeAbleToCommitWhenInDetachedHeadState() {
        Assert.Fail();
        yield return null;
    }

    [TearDown]
    public void AfterEachTest() {

    }
}
