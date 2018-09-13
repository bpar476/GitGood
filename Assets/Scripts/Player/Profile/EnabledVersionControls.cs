using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledVersionControls : Singleton<EnabledVersionControls> {

	public bool CanCommit {
		get {
			return _canCommit;
		}
	}
	private bool _canCommit = false;

	public void EnableCommit() {
		_canCommit = true;
	}

	public bool CanBranch {
		get {
			return _canBranch;
		}
	}
	private bool _canBranch = false;

	public void EnableBranch() {
		_canBranch = true;
	}

	public bool CanReset {
		get {
			return _canReset;
		}
	}
	private bool _canReset = false;

	public void EnableReset() {
		_canReset = true;
	}

	public bool CanMerge {
		get {
			return _canMerge;
		}
	}
	private bool _canMerge = false;

	public void EnableMerge() {
		_canMerge = true;
	}

	public bool CanCheckout {
		get {
			return _canCheckout;
		}
	}
	private bool _canCheckout = false;

	public void EnableCheckout() {
		_canCheckout = true;
	}
}
