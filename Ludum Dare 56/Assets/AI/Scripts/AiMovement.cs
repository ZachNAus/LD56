using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public float StunnedDuration { get; private set; }
	State beforeState;

	private void Awake()
	{
		activationItem.Movement = this;
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

				if(StunnedDuration <= 0)
				{
					activationItem.StopStun();

					CurrentState = beforeState;
				}
				break;
		}
	}

	void TryActivate()
	{
		switch (activationType)
		{
			case ActivationType.PlayerWithinRange:
				var distToPlayer = (PlayerStats.instance.transform.position - transform.position).sqrMagnitude;
				if(distToPlayer < activateRange * activateRange)
					Activate();
				break;
		}
	}
	void TryDeactivate()
	{
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
	public void Stun(float duration = 1)
	{
		if (CurrentState != State.Stunned)
		{
			beforeState = CurrentState;

			activationItem.Stun();
		}

		StunnedDuration += duration;
		CurrentState = State.Stunned;
	}
}
