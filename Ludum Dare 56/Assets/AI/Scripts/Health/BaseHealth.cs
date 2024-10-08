using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour, IHasHealth
{
	[SerializeField] float maxHealth;

	[SerializeField] protected Image healthBar;

	[SerializeField] float invulnTime;
	float timeLastHit;

	[SerializeField] AudioClip onDeathSFX;
	[SerializeField] AudioClip[] onHitSFX;

	public UnityEvent<Transform, bool> OnTakedamage;
	public UnityEvent onDeath;

	public float MaxHealth => maxHealth;
	public float CurrentHealth { get; set; }

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

		if(Time.time - timeLastHit < invulnTime)
		{
			return;
		}
		timeLastHit = Time.time;

		CurrentHealth -= damage;
		CurrentHealth = Mathf.Max(0, CurrentHealth);

		if (healthBar)
			healthBar.fillAmount = CurrentHealth / MaxHealth;

		if(damage > 0)
			OnTakedamage?.Invoke(source, doKnockback);

		if (onHitSFX != null && onHitSFX.Length > 0)
			GetComponent<AudioSource>()?.PlayOneShot(onHitSFX[Random.Range(0, onHitSFX.Length)]);

		if (IsDead)
			Die();
	}

	public virtual void Die()
	{
		OnDeath?.Invoke();

		if (onDeathSFX)
			GetComponent<AudioSource>()?.PlayOneShot(onDeathSFX);
	}
}