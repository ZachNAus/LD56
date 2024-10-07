using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Something the player can pickup.
/// TODO: Make this an interaction? Interaction system?
/// </summary>
public class Pickup : MonoBehaviour
{
	[Tooltip("Holdable to spawn when the player picks this pick up.")]
	public Holdable holdablePrefab;

	public UnityEvent ongrabbed;

	public virtual Holdable Pick()
	{
		ongrabbed?.Invoke();

		return Instantiate(holdablePrefab);
	}
}