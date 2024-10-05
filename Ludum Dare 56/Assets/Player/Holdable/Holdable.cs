using System;
using UnityEngine;

/// <summary>
/// Something that the player holds.
/// </summary>
public abstract class Holdable : MonoBehaviour
{
	[NonSerialized] public Player player;
	[Tooltip("If set, the holdable will be dropable, and this will be spawned.")]
	public GameObject spawnWhenDropped;

	/// <summary>
	/// Called when the player starts holding this holdable.
	/// </summary>
	public virtual void OnEnter() { }
	/// <summary>
	/// Called when the player stops holding this holdable.
	/// </summary>
	public virtual void OnExit() { }

	/// <summary>
	/// Called when the player uses this holdable
	/// i.e. they click the mouse.
	/// </summary>
	/// <param name="down">true if mouse down, false if up</param>
	public virtual void OnUse(bool down) { }

	public virtual void Drop()
	{
		if (spawnWhenDropped != null)
		{
			// Spawn drop.
			Instantiate(spawnWhenDropped, player.transform.position - player.transform.right, player.transform.rotation);
		}
	}
}

/// <summary>
/// Something that can be hit.
/// </summary>
public interface IHittable
{
	/// <param name="holdable">The thing that hit us.</param>
	public void OnHit(Holdable holdable);

	public static bool IsHittable(GameObject other, out IHittable hittable)
	{
		hittable = other.GetComponentInParent<IHittable>();
		return hittable != null;
	}
}