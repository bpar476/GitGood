using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVersionManager {
	void Stage(GameObject gobj);

	void Commit(string message);

	void ResetToHead(GameObject gobj);
}