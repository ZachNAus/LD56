using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
	[SerializeField] CanvasGroup target;

	// Start is called before the first frame update
	void Start()
	{
		PlayerStats.instance.OnTakedamage.AddListener(OnTakeDmg);
	}

	void OnTakeDmg(Transform t, bool b)
	{
		float alpha = 0;
		DOTween.To(() => alpha, x => alpha = x, 1, 0.2f).OnUpdate(() =>
		{
			target.alpha = alpha;
		}).OnComplete(() =>
		{
			DOTween.To(() => alpha, x => alpha = x, 0, 0.2f).OnUpdate(() =>
			{
				target.alpha = alpha;
			});
		});
	}
}
