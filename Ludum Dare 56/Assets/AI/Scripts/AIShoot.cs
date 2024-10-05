using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : AIChase
{
	public enum ShootCondition
	{
		AfterDelay,
		CloseToPlayer
	}

	[Header("Shoot")]
	[SerializeField] ShootCondition shootingCondition;

	[SerializeField] Projectile projectileToSummon;
	[SerializeField] Transform spawnPos;

	[Space]

	[SerializeField] float stopTimeWhenShooting;

	[Min(0)] [SerializeField] float shootDelay;

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

			switch (shootingCondition)
			{
				case ShootCondition.AfterDelay:
					if (timeTillNextShoot <= 0)
					{
						Shoot();
						timeTillNextShoot = shootDelay;
					}
					break;
				case ShootCondition.CloseToPlayer:
					if (timeTillNextShoot <= 0)
					{
						if ((PlayerStats.instance.transform.position - transform.position).sqrMagnitude < 9/*3x3*/)
						{
							Shoot();
							timeTillNextShoot = shootDelay;
						}
					}
					break;
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

		if (proj.ParentProjectile)
			proj.transform.parent = spawnPos;

		proj.Caster = GetComponent<IHasHealth>();
		proj.transform.position = spawnPos.position;
		proj.transform.eulerAngles = spawnPos.eulerAngles;


		if(stopTimeWhenShooting > 0)
		{
			StopMoving();

			StartCoroutine(PerformActionAfterDelay(stopTimeWhenShooting, StartMoving));
		}
	}

	IEnumerator PerformActionAfterDelay(float delay, System.Action action)
	{
		yield return new WaitForSeconds(delay);

		action?.Invoke();
	}
}
