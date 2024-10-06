using System;
using UnityEngine;

public class HoldableNeedleSword : Holdable
{
	public Transform model;
	public SetActiveDuration hitbox;
	public float cooldown;

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
			swingTime = Time.time;
			// Align player rotation to camera.
			player.transform.rotation = Quaternion.LookRotation(player.playerCamera.transform.forward);
			player.PlayTorso("Slash");
			hitbox.Show();
			// TODO: Disallow while jumping?
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<IHasHealth>(out var h))
		{
			// TODO: Pass the player here instead?
			if(h != PlayerStats.instance)
				h.TakeDamage(1, transform, true);
		}
	}
}