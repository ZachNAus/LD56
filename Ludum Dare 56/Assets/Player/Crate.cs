using DG.Tweening;
using UnityEngine;

public class Crate : MonoBehaviour, IHittable
{
	public float rotateStrength;

	public void OnHit(Holdable holdable)
	{
		transform.DOKill();
		var r = transform.rotation;
		transform.DOPunchRotation(Vector3.one * rotateStrength, 1f).OnKill(() => transform.rotation = r);
	}
}