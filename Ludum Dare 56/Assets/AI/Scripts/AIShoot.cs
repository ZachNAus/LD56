using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : AIChase
{
	[Header("Shoot")]
	[SerializeField] GameObject projectileToSummon;
	[SerializeField] Transform spawnPos;
	
	[Min(0)][SerializeField] float shootDelay;

	bool CountDown = false;
	float timeTillNextShoot;

	public override void Activate()
	{
		base.Activate();
		CountDown = true;

		timeTillNextShoot = shootDelay;
	}

	public override void ActivateUpdate()
	{
		base.ActivateUpdate();

		if (CountDown)
		{
			timeTillNextShoot -= Time.deltaTime;

			if(timeTillNextShoot <= 0)
			{
				Shoot();
				timeTillNextShoot = shootDelay;
			}
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();

		CountDown = false;
	}
	public override void Stun()
	{
		base.Stun(); 
		CountDown = false;
	}
	public override void StopStun()
	{
		base.StopStun();
		CountDown = true;
	}

	void Shoot()
	{
		var proj = Instantiate(projectileToSummon);
		proj.transform.position = spawnPos.position;
		proj.transform.eulerAngles = spawnPos.eulerAngles;
	}
}
