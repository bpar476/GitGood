using System.Collections.Generic;
using UnityEngine;

public interface IVisionObserver {
    void ProcessVisibleObject(GameObject gobj);
    void ObjectEnteredVisibility(GameObject gobj);
    void ObjectLeftVisibility(GameObject gobj);
}
