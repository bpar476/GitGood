using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class VersionManagerTests {

    [Test]
    public void NewTestScriptSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestResetPositionToHead() {
        // Given
        VersionController testObject = new GameObject().AddComponent<VersionController>();
        testObject.AddVersionable(new TransformVersionable(testObject.gameObject));

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

    [TearDown]
    public void AfterEachTest() {
        
    }
}
