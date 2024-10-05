using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HoldableStick : Holdable
{
	public Hitbox hitbox;
	public Transform model;

	private Coroutine attackingRoutine;

	private void Awake()
	{
		hitbox.onEnter += c =>
		{
			if (IHittable.IsHittable(c.gameObject, out var hittable))
			{
				hittable.OnHit(this);
			}
		};
	}

	private void OnDisable()
	{
		attackingRoutine = null;
	}

	public override void OnUse(bool down)
	{
		if (!down) return;

		if (attackingRoutine == null)
		{
			attackingRoutine = StartCoroutine(AttackRoutine());
		}

		IEnumerator AttackRoutine()
		{
			hitbox.gameObject.SetActive(true);
			model.DOPunchScale(Vector3.one * 0.2f, 0.5f);
			yield return new WaitForSeconds(0.5f);
			hitbox.gameObject.SetActive(false);
			attackingRoutine = null;
		}
	}
}