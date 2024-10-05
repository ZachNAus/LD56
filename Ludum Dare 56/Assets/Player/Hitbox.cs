using System;
using UnityEngine;

/// <summary>
/// Hits something.
/// </summary>
public class Hitbox : MonoBehaviour
{
	public Action<Collider> onEnter;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Hitbox: " + other.gameObject);
		onEnter?.Invoke(other);
	}
}