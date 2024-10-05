using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHasHealth
{
	public float MaxHealth { get; }
	public float CurrentHealth { get; }
	public UnityEvent OnDeath { get; }

	public void TakeDamage(float damage,Transform source, bool doKnockback);

	public void Die();

}
