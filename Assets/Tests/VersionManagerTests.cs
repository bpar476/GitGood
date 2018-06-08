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
    public IEnumerator TestResetPlayerPosition() {
        // Given
        GameObject testObject = new GameObject();

        IVersionManager versionManager = new GameObject().AddComponent<InMemoryVersionManager>();
        testObject.transform.position = new Vector2(0,0);
        versionManager.Stage(testObject);
        versionManager.Commit("Set thing position to 0,0");

        yield return null;

        // When
        testObject.transform.position = new Vector2(1,0);
        versionManager.ResetToHead(testObject);

        yield return null;

        // Then
        Assert.AreEqual(testObject.transform.position.x, 0.0f, 0.1f);
        Assert.AreEqual(testObject.transform.position.y, 0.0f, 0.1f);
    }

    [TearDown]
    public void AfterEachTest() {
        
    }
}
