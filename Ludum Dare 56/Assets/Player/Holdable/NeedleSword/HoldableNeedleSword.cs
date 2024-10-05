using UnityEngine;

public class HoldableNeedleSword : Holdable
{
	public Transform model;

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
			// TODO: Damage, etc.
			// TODO: Disallow while jumping?
		}
	}
}