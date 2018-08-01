using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactables are game objects which the player can interact with by pressing the interact key.
/// </summary>
public abstract class Interactable : MonoBehaviour {

	/// <summary>
	/// Attempts to carry out the action that happens when the player interacts with it.
	/// Each Interactable should do its own checking for if the actor is in the right state
	/// to act with it.
	/// </summary>
	/// <param name="actor">The object trying to interact with the interactable (usually the player)</param>
	/// <returns>true if the actor was able to interact with the object, false otherwise</returns>
	public abstract bool TryInteract(GameObject actor);
}
