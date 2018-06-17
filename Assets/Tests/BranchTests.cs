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
}
