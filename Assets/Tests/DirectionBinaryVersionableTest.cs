using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class DirectionVersionableTest {

    [UnityTest]
    public IENumerator ShouldSetLocalScale() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createBinaryVersionable();
        GameObject testObject = testController.GetActiveVersion();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        Assert.AreEqual(testObject.transform.localScale.x, 1.0f, 0.01f);

        versionManager.Add(testController);
        ICommit commit = versionManager.Commit("Add a box");

        testObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        Assert.AreEqual(testObject.transform.localScale.x, -1.0f, 0.01f);

        versionManager.Add(testController);
        versionManager.Commit("Flip the box");

        versionManager.CheckoutCommit(commit);

        Assert.AreEqual(testObject.transform.localScale.x, 1.0f, 0.01f);
    }

}