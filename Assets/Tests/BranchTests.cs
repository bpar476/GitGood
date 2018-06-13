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
}
