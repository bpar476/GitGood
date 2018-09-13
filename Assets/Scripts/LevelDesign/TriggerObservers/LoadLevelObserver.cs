using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelObserver : PositiveTriggerObserver {

	public int levelCode;
	public LevelControl levelController;

	protected override void HandleTrigger(bool state) {
		this.levelController.SwitchLevel(this.levelCode);
	}
}
