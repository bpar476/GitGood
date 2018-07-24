using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class VersionManagerTests {
    [SetUp]
    public void SetUp() {
        VersionManager.Reset();
    }


    [UnityTest]
    public IEnumerator ShouldAllowRemovalOfStagedObjectFromStagingArea() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionManager.Instance().Add(testController);

        Assert.True(VersionManager.Instance().IsObjectStaged(testController));

        VersionManager.Instance().Unstage(testController);

        Assert.False(VersionManager.Instance().IsObjectStaged(testController));

        yield return null;
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestResetPositionToHead() {
        // Given
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionablePlayer();

        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0,0);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Commit("Set thing position to 0,0");

        yield return null;

        // When
        testObject.transform.position = new Vector2(1,0);
        VersionManager.Instance().ResetToHead(testController);

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

        testObject.transform.position = new Vector2(0.0f,0.0f);

        VersionManager.Instance().Add(testController);
        ICommit commitToLoad = VersionManager.Instance().Commit("Set thing position to 0,0");

        yield return null;
        
        testObject.transform.position = new Vector2(1.0f,1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Commit("Set thing position to 1,1");

        yield return null;
        
        // When
        VersionManager.Instance().CheckoutCommit(commitToLoad);

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

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        yield return null;

        VersionManager.Instance().ResetToHead();

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

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(0.0f, 0.0f);

        yield return null;

        VersionManager.Instance().ResetToHead(testController);

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

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(-2.0f, -2.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Commit("Move one of the objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(6.0f, 0.0f);

        yield return null;

        VersionManager.Instance().ResetToHead();

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

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        Guid firstCommitId = VersionManager.Instance().Commit("Create two boxes").GetId();

        yield return null;

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Move boxes");

        yield return null;

        VersionManager.Instance().Checkout(VersionManager.Instance().GetActiveBranch(), firstCommitId);

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

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        Guid firstCommitId = VersionManager.Instance().Commit("Create two boxes").GetId();

        yield return null;

        VersionManager.Instance().CreateBranch("feature");
        VersionManager.Instance().Checkout("feature");

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Move boxes");

        yield return null;

        VersionManager.Instance().Checkout("master", firstCommitId);

        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(3.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestObject.transform.position.y, 0.1f);

        IBranch master = VersionManager.Instance().LookupBranch("master");
        Assert.AreEqual(master, VersionManager.Instance().GetActiveBranch());
    }

    [UnityTest]
    public IEnumerator shouldResetObjectToInitialPositionIfCheckingOutCommitWhereItWasNotTracked() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        VersionManager.Instance().Add(testController);

        Guid firstCommitId = VersionManager.Instance().Commit("Create a box").GetId();

        yield return null;

        VersionController otherTestController = factory.createVersionableBox();
        GameObject otherTestObject = otherTestController.GetActiveVersion();
        
        otherTestController.GetComponent<TransformVersionable>().SetInitialState(new Vector2(5.0f, 0.0f));

        testObject.transform.position = new Vector2(-3.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, -2.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create another box and move the first box");

        yield return null;

        VersionManager.Instance().Checkout(VersionManager.Instance().GetActiveBranch(), firstCommitId);

        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(5.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestController.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator shouldResetTrackedObjectsWhenCheckingOutCommitWhereAnObjectWasNotTracked() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        VersionManager.Instance().Add(testController);

        Guid firstCommitId = VersionManager.Instance().Commit("Create a box").GetId();

        yield return null;

        VersionController otherTestController = factory.createVersionableBox();
        GameObject otherTestObject = otherTestController.GetActiveVersion();
        
        testObject.transform.position = new Vector2(-3.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, -2.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create another box and move the first box");

        yield return null;

        VersionManager.Instance().Checkout(VersionManager.Instance().GetActiveBranch(), firstCommitId);

        Assert.False(VersionManager.Instance().IsObjectTracked(otherTestController));
    }

    [UnityTest]
    public IEnumerator shouldReloadTrackedObjectsWhenCheckingOutCommitWithNewTrackedObject() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        GameObject testObject = testController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);

        VersionManager.Instance().Add(testController);

        Guid firstCommitId = VersionManager.Instance().Commit("Create a box").GetId();

        yield return null;

        VersionController otherTestController = factory.createVersionableBox();
        GameObject otherTestObject = otherTestController.GetActiveVersion();
        
        testObject.transform.position = new Vector2(-3.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, -2.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        Guid secondCommitId = VersionManager.Instance().Commit("Create another box and move the first box").GetId();

        yield return null;

        VersionManager.Instance().Checkout(VersionManager.Instance().GetActiveBranch(), firstCommitId);

        Assert.False(VersionManager.Instance().IsObjectTracked(otherTestController));

        VersionManager.Instance().Checkout(VersionManager.Instance().GetActiveBranch(), secondCommitId);

        Assert.True(VersionManager.Instance().IsObjectTracked(otherTestController));
    }

    [UnityTest]
    public IEnumerator shouldNotBeAbleToCommitWhenInDetachedHeadState() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        ICommit firstCommit = VersionManager.Instance().Commit("Create two boxes");
        Guid firstCommitId = firstCommit.GetId();

        yield return null;

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Move boxes");

        yield return null;

        VersionManager.Instance().Checkout("master", firstCommitId);

        testObject.transform.position = new Vector2(3.0f, 3.0f);

        VersionManager.Instance().Add(testController);

        ICommit commit = null;
        try {
            commit = VersionManager.Instance().Commit("Move the box");
            Assert.Fail();
        } catch (InvalidOperationException ioe) {
            Assert.AreEqual(ioe.Message, "Cannot commit in detached HEAD state");
            Assert.IsNull(commit);
            Assert.AreEqual(VersionManager.Instance().GetActiveCommit(), firstCommit);
        }
    }

    [UnityTest]
    public IEnumerator shouldBeAbleToCreateNewBranchWhenInDetachedHeadAndThenCommit() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        ICommit firstCommit = VersionManager.Instance().Commit("Create two boxes");
        Guid firstCommitId = firstCommit.GetId();

        yield return null;

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Move boxes");

        yield return null;

        VersionManager.Instance().Checkout("master", firstCommitId);

        IBranch newBranch = VersionManager.Instance().CreateBranch("refactor");
        VersionManager.Instance().Checkout(newBranch);

        Assert.AreEqual(newBranch.GetTip().GetId(), VersionManager.Instance().GetActiveBranch().GetTip().GetId());
        Assert.AreEqual(newBranch.GetTip().GetId(), VersionManager.Instance().GetActiveCommit().GetId());

        testObject.transform.position = new Vector2(-1.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        ICommit commit = VersionManager.Instance().Commit("Move a box to the left");

        Assert.AreEqual(newBranch.GetTip().GetId(), commit.GetId());
        Assert.True(commit.ObjectIsTrackedInThisCommit(testController));
        Assert.True(commit.ObjectIsTrackedInThisCommit(otherTestController));
    }

    [UnityTest]
    public IEnumerator shouldPreserveStagingAreaWhenCheckingOutNewBranch() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createVersionableBox();
        VersionController otherTestController = factory.createVersionableBox();

        GameObject testObject = testController.GetActiveVersion();
        GameObject otherTestObject = otherTestController.GetActiveVersion();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 0.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().Commit("Create two boxes").GetId();

        yield return null;

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(4.0f, 1.0f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Add(otherTestController);

        VersionManager.Instance().CreateBranch("feature");
        VersionManager.Instance().Checkout("feature");

        VersionManager.Instance().Commit("Move boxes");

        VersionManager.Instance().ResetToHead();

        Assert.AreEqual(1.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(4.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(1.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [TearDown]
    public void AfterEachTest() {

    }
}
