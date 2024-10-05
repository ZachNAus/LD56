using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasHealth
{
	public float MaxHealth { get; }
	public float CurrentHealth { get; }

	public void TakeDamage(float damage);

	public void Die();
}
