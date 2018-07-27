using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class DirectionVersionableTest {
    [SetUp]
    public void SetUp() {
        VersionManager.Reset();
    }

    [UnityTest]
    public IEnumerator ShouldSetLocalScale() {
        VersionableObjectFactory factory = new VersionableObjectFactory();

        VersionController testController = factory.createBinaryVersionable();
        GameObject testObject = testController.GetActiveVersion();

        Assert.AreEqual(1.0f, testObject.transform.localScale.x, 0.01f);

        VersionManager.Instance().Add(testController);
        ICommit commit = VersionManager.Instance().Commit("Add a box");

        yield return null;

        testObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        Assert.AreEqual(-1.0f, testObject.transform.localScale.x,  0.01f);

        VersionManager.Instance().Add(testController);
        VersionManager.Instance().Commit("Flip the box");

        yield return null;

        VersionManager.Instance().CheckoutCommit(commit);

        yield return null;

        Assert.AreEqual(1.0f, testObject.transform.localScale.x, 0.01f);
    }

}
