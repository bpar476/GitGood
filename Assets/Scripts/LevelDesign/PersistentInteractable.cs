using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PersistentInteractables are interactables that stay with the actor when
/// they interact with them; for example, picking up a box. PersistentInteractables
/// need to define logic for ending the interaction.
/// </summary>
public abstract class PersistentInteractable : Interactable {

	/// <summary>
	/// Stops interacting with the interactable.
	/// </summary>
	public abstract void StopInteracting();
}
