using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

	[SerializeField] float maxHealth;
	public float MaxHealth => maxHealth;

	public float CurrentHealth { get; private set; }

	private void Awake()
	{
		instance = this;

		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(float dmg)
	{
		CurrentHealth -= dmg;

		if (CurrentHealth <= 0)
			Die();
	}

	public void Die()
	{

	}
}
