using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Rigidbody rb;
	// Units per frame.
	[NonSerialized] public Vector3 velocity;
	[NonSerialized] public Holdable holdable;

	private void Start()
	{
		Destroy(gameObject, 4f);
	}

	private void FixedUpdate()
	{
		rb.MovePosition(transform.position + velocity * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IHittable.IsHittable(other.gameObject, out var h))
		{
			// Bit scuffed. OnHit should take a gameobject instead?
			h.OnHit(holdable);
		}
		Destroy(gameObject);
	}
}