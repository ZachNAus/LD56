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
		if (chaseRoutine != null)
			StopCoroutine(chaseRoutine);

		chaseRoutine = StartCoroutine(Co_Chase());
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
		if (chaseRoutine != null)
			StopCoroutine(chaseRoutine);

		agent.isStopped = true;
	}

	public override void Stun()
	{
		if (chaseRoutine != null)
			StopCoroutine(chaseRoutine);

		agent.isStopped = true;
	}
	public override void StopStun()
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
