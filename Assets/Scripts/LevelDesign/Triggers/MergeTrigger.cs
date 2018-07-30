using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTrigger : DelegateTrigger {

    protected override bool shouldFire() {
        return true;
    }
}
