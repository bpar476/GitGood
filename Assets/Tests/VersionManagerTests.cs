using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class VersionManagerTests {

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestResetPositionToHead() {
        // Given
        VersionController testObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0,0);

        versionManager.Add(testObject);
        versionManager.Commit("Set thing position to 0,0");

        yield return null;

        // When
        testObject.transform.position = new Vector2(1,0);
        versionManager.ResetToHead(testObject);

        yield return null;

        // Then
        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetPositionBeforeHEAD() {
        // Given
        VersionController testObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f,0.0f);

        versionManager.Add(testObject);
        Commit commitToLoad = versionManager.Commit("Set thing position to 0,0");

        yield return null;
        
        testObject.transform.position = new Vector2(1.0f,1.0f);

        versionManager.Add(testObject);
        versionManager.Commit("Set thing position to 1,1");

        yield return null;
        
        // When
        versionManager.ResetToCommit(commitToLoad);

        yield return null;

        // Then
        Assert.AreEqual(0.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, testObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetMultipleObjectPositions() {
        // Given
        VersionController testObject = createTransformVersionedObject();
        VersionController otherTestObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        versionManager.Add(testObject);
        versionManager.Add(otherTestObject);

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
        VersionController testObject = createTransformVersionedObject();
        VersionController otherTestObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(1.0f, 3.0f);
        otherTestObject.transform.position = new Vector2(-1.0f, 5.0f);

        versionManager.Add(testObject);
        versionManager.Add(otherTestObject);

        versionManager.Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(0.0f, 0.0f);

        yield return null;

        versionManager.ResetToHead(testObject);

        yield return null;

        Assert.AreEqual(1.0f, testObject.transform.position.x, 0.1f);
        Assert.AreEqual(3.0f, testObject.transform.position.y, 0.1f);

        Assert.AreEqual(0.0f, otherTestObject.transform.position.x, 0.1f);
        Assert.AreEqual(0.0f, otherTestObject.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator TestResetObjectsToCommitWhereOneWasNotChanged() {
        VersionController testObject = createTransformVersionedObject();
        VersionController otherTestObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testObject);
        versionManager.Add(otherTestObject);

        versionManager.Commit("Create two objects");

        yield return null;

        testObject.transform.position = new Vector2(-2.0f, -2.0f);

        versionManager.Add(testObject);
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

    [TearDown]
    public void AfterEachTest() {
        
    }

    private VersionController createTransformVersionedObject() {
        VersionController testObject = new GameObject().AddComponent<VersionController>();
        testObject.AddVersionable(new TransformVersionable(testObject.gameObject));
        return testObject;
    }
}
