using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
	#region Enums
	public enum ActivationType
	{
		PlayerWithinRange,
		Manual,
	}
	public enum DeactivationType
	{
		PlayerOutOfRange,
		Never,
	}

	public enum State
	{
		Idle,
		Activated,
		Stunned,
	}
	#endregion

	[ReadOnly]
	public State CurrentState = State.Idle;

	[SerializeField] ActivationType activationType;
	public bool ActivateWithinRange => activationType == ActivationType.PlayerWithinRange;
	[ShowIf(nameof(ActivateWithinRange))]
	[SerializeField] float activateRange = 10;
	[Space]
	[SerializeField] DeactivationType deactivationType;
	public bool DeactivateWithinRange => deactivationType == DeactivationType.PlayerOutOfRange;
	[ShowIf(nameof(DeactivateWithinRange))]
	[SerializeField] float deactivateRange = 20;

	[Space]

	[SerializeField] AIActivationType activationItem;

	[SerializeField] BaseHealth health;

	[SerializeField] Animator animator;

	[SerializeField] SkinnedMeshRenderer mesh;

	[Space]

	[SerializeField] Vector3 knockBackOffset = new Vector3(0, 1, 0);

	[SerializeField] GameObject deathParticles;
	[SerializeField] ParticleSystem dizzyParticles;

	[Header("MIKE ITS HERE")]
	[Tooltip("0 = no jump")]
	[SerializeField] float jumpHeight;
	[SerializeField] int numJumps = 1;
	[SerializeField] float timeToKnockback = 0.6f;
	[SerializeField] float knockbackDist = 3;

	public float StunnedDuration { get; private set; }
	State beforeState;

	private void Awake()
	{
		activationItem.Movement = this;

		health.OnTakedamage.AddListener((t, b) =>
		{
			if (b)
			{
				DoKnockback(t, true);
			}
		});
		health.OnDeath.AddListener(Die);
	}

	private void Update()
	{
		switch (CurrentState)
		{
			case State.Idle:
				TryActivate();
				break;
			case State.Activated:
				activationItem.ActivateUpdate();
				TryDeactivate();
				break;
			case State.Stunned:
				StunnedDuration -= Time.deltaTime;

				if (StunnedDuration <= 0)
				{
					activationItem.StopStun();

					CurrentState = beforeState;
				}
				break;
		}
	}

	void TryActivate()
	{
		if (PlayerStats.instance.IsDead || health.IsDead)
			return;

		switch (activationType)
		{
			case ActivationType.PlayerWithinRange:
				var distToPlayer = (PlayerStats.instance.transform.position - transform.position).sqrMagnitude;
				if (distToPlayer < activateRange * activateRange)
					Activate();
				break;
		}
	}
	void TryDeactivate()
	{
		if (PlayerStats.instance.IsDead || health.IsDead)
		{
			Deactivate();
		}

		switch (deactivationType)
		{
			case DeactivationType.PlayerOutOfRange:
				var distToPlayer = (PlayerStats.instance.transform.position - transform.position).sqrMagnitude;
				if (distToPlayer > deactivateRange * deactivateRange)
					Deactivate();
				break;
		}
	}

	/// <summary>
	/// Enter a state where attacks player
	/// </summary>
	public void Activate()
	{
		CurrentState = State.Activated;

		activationItem.Activate();
	}

	/// <summary>
	/// Go back to waiting
	/// </summary>
	public void Deactivate()
	{
		CurrentState = State.Idle;

		activationItem.Deactivate();
	}

	[Button]
	public void Stun(Transform source, bool doKnockback)
	{
		if (CurrentState != State.Stunned)
		{
			beforeState = CurrentState;

			activationItem.Stun();
		}

		dizzyParticles.Play();

		StunnedDuration += 1.5f;
		CurrentState = State.Stunned;

		if (doKnockback)
		{
			DoKnockback(source, false);
		}
	}

	public void DoKnockback(Transform source, bool stun)
	{
		if (stun)
			activationItem.Stun();

		mesh.material.color = Color.red;
		StartCoroutine(PerformActionAfterDelay(0.2f, () => mesh.material.color = Color.white));

		animator.SetTrigger("Hit");

		var dir = (source.position - transform.position).normalized;

		dir.y = 0;

		RaycastHit rayHit;

		var dist = health.IsDead ? -10 : -knockbackDist;

		var endPosition = transform.position + (dir * dist);

		if (Physics.Raycast(endPosition + Vector3.up * 50, Vector3.down, out rayHit))
		{
			endPosition = rayHit.point;
		}

		//if (NavMesh.SamplePosition(endPosition, out var hit, 10, -1))
		//{
		//	endPosition = hit.position;
		//}

		transform.DOJump(endPosition + knockBackOffset, jumpHeight, numJumps, timeToKnockback).OnComplete(() =>
		{
			if (stun)
				activationItem.StopStun();
		});
	}

	void Die()
	{
		//var dir = (transform.position - PlayerStats.instance.transform.position).normalized;
		transform.DORotate(new Vector3(-720, 0, 0), timeToKnockback, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() =>
		{
			var inst = Instantiate(deathParticles);
			inst.transform.position = transform.position;
			Destroy(inst, 5);

			Destroy(gameObject);
		});
	}

	IEnumerator PerformActionAfterDelay(float delay, System.Action action)
	{
		yield return new WaitForSeconds(delay);

		action?.Invoke();
	}
}
