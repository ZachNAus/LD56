using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base type for the activation

public abstract class AIActivationType : MonoBehaviour
{
	public AiMovement Movement;

	public abstract void Activate();

	public abstract void ActivateUpdate();

	public abstract void Deactivate();

	public abstract void Stun();
	public abstract void StopStun();
}
