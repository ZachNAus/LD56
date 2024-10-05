using UnityEngine;

/// <summary>
/// Something the player can pickup.
/// TODO: Make this an interaction? Interaction system?
/// </summary>
public class Pickup : MonoBehaviour
{
	[Tooltip("Holdable to spawn when the player picks this pick up.")]
	public Holdable holdablePrefab;

	public virtual Holdable Pick()
	{
		return Instantiate(holdablePrefab);
	}
}