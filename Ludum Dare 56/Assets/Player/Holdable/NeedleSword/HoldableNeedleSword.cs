using System;
using UnityEngine;

public class HoldableNeedleSword : Holdable
{
	public Transform model;
	public SetActiveDuration hitbox;
	public float cooldown;

	public int damage;

	private float swingTime;

	public override void OnEnter()
	{
		model.transform.SetParent(player.ModelRightHand(), false);
	}

	public override void OnExit()
	{
		Destroy(model.gameObject);
	}

	public override void OnUse(bool down)
	{
		if (down && (Time.time - swingTime) >= cooldown)
		{
			hitPeople.Clear();
			swingTime = Time.time;
			// Align player rotation to camera.
			player.transform.rotation = Quaternion.LookRotation(player.playerCamera.transform.forward.OnlyXZ());
			player.PlayTorso("Slash");
			hitbox.Show();
			// TODO: Disallow while jumping?
		}
	}

	System.Collections.Generic.List<IHasHealth> hitPeople = new System.Collections.Generic.List<IHasHealth>();

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<IHasHealth>(out var h))
		{
			// TODO: Pass the player here instead?
			if(h != PlayerStats.instance && hitPeople.Contains(h) == false)
			{
				hitPeople.Add(h);
				h.TakeDamage(damage, transform, true);
			}
		}
	}
}