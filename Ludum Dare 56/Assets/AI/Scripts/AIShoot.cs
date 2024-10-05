using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : AIChase
{
	[Header("Shoot")]
	[SerializeField] GameObject projectileToSummon;
	[SerializeField] float shootDelay;


	public override void Activate()
	{
		base.Activate();
	}

	public override void ActivateUpdate()
	{
		base.ActivateUpdate();
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}
}
