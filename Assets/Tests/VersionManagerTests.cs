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
        GameObject player = Object.Instantiate(Resources.Load("Tests/PrototypePlayer")) as GameObject;

        IVersionManager versionManager = player.AddComponent<InMemoryVersionManager>();
        player.transform.position = new Vector2(0,0);
        versionManager.Stage();
        versionManager.Commit("Set player position to 0,0");

        yield return null;

        // When
        player.transform.position = new Vector2(1,0);
        versionManager.ResetToHead(player);

        yield return null;

        // Then
        Assert.AreEqual(player.transform.position.x, 0.0f, 0.1f);
        Assert.AreEqual(player.transform.position.y, 0.0f, 0.1f);
    }

    [TearDown]
    public void AfterEachTest() {
        
    }
}
