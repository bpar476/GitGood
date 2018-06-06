using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewTestScript {

    [Test]
    public void NewTestScriptSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestResetPlayerPosition() {
        // Given
        GameObject versionManager = new GameObject().AddComponent<VersionManager>();

        GameObject player = Object.Instantiate(Resources.Load("Tests/PrototypePlayer")) as GameObject;
        player.transform.position = new Vector2(0,0);
        versionManager.Stage(player);
        versionManager.Commit("Set player position to 0,0");

        yield return null;

        // When
        player.transform.position = new Vector2(1,0);
        versionManager.ResetToHead(player);

        yield return null;

        // Then
        Assert.AreEqual(player.transform.position, new Vector2(0,0));
    }

    [TearDown]
    public void AfterEachTest() {
        
    }
}
