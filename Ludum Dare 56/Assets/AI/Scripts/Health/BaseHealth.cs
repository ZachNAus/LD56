using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour, IHasHealth
{
	[SerializeField] float maxHealth;

	[SerializeField] Image healthBar;

	public UnityEvent<Transform, bool> OnTakedamage;
	public UnityEvent onDeath;

	public float MaxHealth => maxHealth;
	public float CurrentHealth { get; private set; }

	public bool IsDead => CurrentHealth <= 0;

	public UnityEvent OnDeath => onDeath;

	protected virtual void Awake()
	{
		CurrentHealth = MaxHealth;

		if (healthBar)
			healthBar.fillAmount = CurrentHealth / MaxHealth;
	}

	[Button]
	public virtual void TakeDamage(float damage, Transform source, bool doKnockback)
	{
		if (IsDead || damage == 0) return;

		CurrentHealth -= damage;
		CurrentHealth = Mathf.Max(0, CurrentHealth);

		if (healthBar)
			healthBar.fillAmount = CurrentHealth / MaxHealth;

		OnTakedamage?.Invoke(source, doKnockback);

		if (IsDead)
			Die();
	}

	public virtual void Die()
	{
		OnDeath?.Invoke();
	}
}