using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : AIActivationType
{
	[SerializeField] NavMeshAgent agent;

	[Tooltip("How far away from the player does this AI want to be")]
	[SerializeField] float preferredDistance;

	Coroutine chaseRoutine;

	public override void Activate()
	{
		StartMoving();
	}

	public override void ActivateUpdate()
	{
		if (!movingForward)
		{
			var dir = (PlayerStats.instance.transform.position - transform.position).normalized;
			dir.y = 0;
			transform.forward = dir;
		}
	}

	public override void Deactivate()
	{
		StopMoving();
	}

	public override void Stun()
	{
		StopMoving();
	}
	public override void StopStun()
	{
		StartMoving();
	}

	protected void StopMoving()
	{
		agent.isStopped = true;

		if (chaseRoutine != null)
			StopCoroutine(chaseRoutine);
	}
	protected void StartMoving()
	{
		chaseRoutine = StartCoroutine(Co_Chase());
	}

	bool movingForward;
	IEnumerator Co_Chase()
	{
		var wait = new WaitForSeconds(0.1f);
		agent.isStopped = false;

		while (true)
		{
			var distSquared = (PlayerStats.instance.transform.position - transform.position).sqrMagnitude;
			movingForward = distSquared > preferredDistance * preferredDistance;
			if (movingForward)
			{
				agent.updateRotation = true;

				agent.SetDestination(PlayerStats.instance.transform.position);
			}
			else
			{
				if(distSquared < (preferredDistance - 3) * (preferredDistance - 3))
				{
					//Back up
					var dir = (PlayerStats.instance.transform.position - transform.position).normalized;

					var endPos = transform.position + dir * -10;

					agent.updateRotation = false;

					agent.SetDestination(endPos);
				}
				else
				{
					agent.SetDestination(transform.position);
				}
			}

			yield return wait;
		}
	}
}
