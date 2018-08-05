using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeObjectController : MonoBehaviour {

	public VersionController underlyingObject {

		get {
			return _underlyingObject;
		}

		set {
			_underlyingObject = value;
			transform.Find("Object").GetComponent<Text>().text = value.gameObject.name;
			transform.Find("Version 1").GetComponent<Text>().text = "branch1";
			transform.Find("Version 2").GetComponent<Text>().text = "branch2";
		}
	}

	private VersionController _underlyingObject;

}
