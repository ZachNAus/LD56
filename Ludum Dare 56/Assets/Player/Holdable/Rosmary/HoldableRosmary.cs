using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldableRosmary : Holdable
{
	public Transform model;
	public SetActiveDuration hitbox;
	public float cooldown;

	public int damage = 2;

	public AudioClip[] onAtk;

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

			PlayerStats.instance.audioSource.PlayOneShot(onAtk[UnityEngine.Random.Range(0, onAtk.Length)]);
		}
	}

	private List<IHasHealth> hitPeople = new();
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<IHasHealth>(out var h))
		{
			if (h != PlayerStats.instance && hitPeople.Contains(h) == false)
			{
				hitPeople.Add(h);
				h.TakeDamage(damage, transform, true);
			}
		}
	}
}