using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BranchTests {
    [UnityTest]
    public IEnumerator TestSimple() {
        Commit a = new Commit(null, "Commit A");
        Commit b = new Commit(a, "Commit B");

        yield return null;
        Assert.AreEqual("Commit A", b.GetParent().GetMessage());
    }

    [UnityTest]
    public IEnumerator TestBranchConstruction() {
        ICommit A = new Commit(null, "Test commit object construction");
        IBranch testBranch = new Branch("feature/branches", A);
        yield return null;
        Assert.AreSame(A, testBranch.GetTip());
        Assert.AreEqual("feature/branches", testBranch.GetName());

    }

    [UnityTest]
    public IEnumerator TestSwitchBranch() {
         VersionController testObject = createTransformVersionedObject();
        VersionController otherTestObject = createTransformVersionedObject();

        VersionManager versionManager = new GameObject().AddComponent<VersionManager>();

        testObject.transform.position = new Vector2(0.0f, 0.0f);
        otherTestObject.transform.position = new Vector2(3.0f, 3.0f);

        versionManager.Add(testObject);
        versionManager.Add(otherTestObject);

        ICommit commit = versionManager.Commit("Add two objects");

        yield return null;
        Assert.AreSame(commit, versionManager.GetActiveBranch().GetTip());

        IBranch testBranch = versionManager.CreateBranch("testBranch");
        Assert.AreEqual("testBranch", testBranch.GetName());
        Assert.AreEqual(true, versionManager.Checkout("testBranch"));
        Assert.AreSame(testBranch, versionManager.GetActiveBranch());
        Assert.AreSame(commit, versionManager.GetActiveBranch().GetTip());

        testObject.transform.position = new Vector2(1.0f, 0.0f);
        versionManager.Add(testObject);
        ICommit secondCommit = versionManager.Commit("Move testObject");

        Assert.AreSame(secondCommit, versionManager.GetActiveBranch().GetTip());
        Assert.AreSame(secondCommit, versionManager.GetHead());
        Assert.AreSame(commit, versionManager.LookupBranch("master").GetTip());

        versionManager.Checkout("master");
        Assert.AreSame(commit, versionManager.GetHead());


    }


    private VersionController createTransformVersionedObject() {
        VersionController testObject = new GameObject().AddComponent<VersionController>();
        testObject.AddVersionable(new TransformVersionable(testObject.gameObject));
        return testObject;
    }
}
