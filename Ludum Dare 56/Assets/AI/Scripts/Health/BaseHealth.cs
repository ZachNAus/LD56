using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour, IHasHealth
{
	[SerializeField] float maxHealth;

	[SerializeField] Image healthBar;

	public UnityEvent OnTakedamage;

	public float MaxHealth => maxHealth;
	public float CurrentHealth { get; private set; }

	public bool IsDead => CurrentHealth <= 0;

	protected virtual void Awake()
	{
		CurrentHealth = MaxHealth;

		if (healthBar)
			healthBar.fillAmount = CurrentHealth / MaxHealth;
	}

	public virtual void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
		
		if (healthBar)
			healthBar.fillAmount = CurrentHealth / MaxHealth;

		OnTakedamage?.Invoke();

		if (IsDead)
			Die();
	}

	public virtual void Die()
	{
	}
}
