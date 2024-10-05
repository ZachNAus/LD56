using UnityEngine;

public class HoldableNeedleSword : Holdable
{
	public Transform model;
	public SetActiveDuration hitbox;

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
		if (down)
		{
			// TODO: Cooldown, wait until we can swing again.
			player.PlayTorso("Slash");
			hitbox.Show();
			// TODO: Damage, etc.
			// TODO: Disallow while jumping?
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<IHasHealth>(out var h))
		{
			h.TakeDamage(1);
		}
	}
}