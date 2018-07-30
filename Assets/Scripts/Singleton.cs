using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    protected static T singletonInstance;
    public static T Instance() {
        if (singletonInstance == null) {
            new GameObject().AddComponent<T>();
        }

        return singletonInstance;
    }

    public static void Reset() {
		singletonInstance = null;
		new GameObject().AddComponent<T>();
	}

	protected virtual void Awake() {
		if (singletonInstance == null) {
			singletonInstance = (T)this;
		}
		else if (singletonInstance != this) {
			Destroy(gameObject);
			return;
		}
	}
}
