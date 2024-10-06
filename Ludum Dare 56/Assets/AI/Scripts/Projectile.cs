using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
	public enum MoveType
	{
		MoveForward,
		HomeOnPlayer,
		Roll,
	}

	[SerializeField] MoveType moveType;
	public MoveType MovementType => moveType;
	bool Homing => moveType == MoveType.HomeOnPlayer;

	[SerializeField] float lifeTime = -1;

	[SerializeField] float moveSpeed = 5;

	[Space]

	[SerializeField] float damage = 10;

	[Header("Other")]

	[SerializeField] bool doKnockback;

	[ShowIf(nameof(Homing))]
	[SerializeField] float rotateSpeed = 10;

	public bool ParentProjectile;

	float TimeAlive;

	public IHasHealth Caster { get; set; }

	private void Update()
	{
		if (lifeTime > 0)
		{
			TimeAlive += Time.deltaTime;
			if (TimeAlive > lifeTime)
			{
				DestroyProj();
			}
		}

		switch (moveType)
		{
			case MoveType.MoveForward:
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				break;
			case MoveType.HomeOnPlayer:
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				transform.rotation = Quaternion.LookRotation(
					Vector3.RotateTowards(
						transform.forward,
						PlayerStats.instance.transform.position - transform.position,
						rotateSpeed * Time.deltaTime,
						0.0f));
				break;
		}
	}

	public void DestroyProj()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IHasHealth health))
		{
			if (Caster != health)
			{
				health.TakeDamage(damage, transform, doKnockback);
				DestroyProj();

				float res = Player.instance.TakeDamage(damage, transform);

				if (health == PlayerStats.instance && ParentProjectile && res == 0)
				{
					(Caster as Component).GetComponent<AiMovement>().Stun(other.transform, true);
				}
			}
		}
	}
}
