using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI txt;
	[SerializeField] CanvasGroup fadeArea;

	[Space]

	[SerializeField] float showNextCharDelay = 0.025f;
	float timeOfLastChar;

	float ExtraLifeTime;

	[Button]
	public void SaySomething(string msg, float extraLifeTime)
	{
		ExtraLifeTime = extraLifeTime;
		txt.maxVisibleCharacters = 0;
		txt.SetText(msg);

		finished = false;

		float alpha = 0;
		DOTween.To(() => alpha, x => alpha = x, 1, 0.3f).OnUpdate(() => 
		{
			fadeArea.alpha = alpha;
		});
	}

	bool finished;

	private void Update()
	{
		if (txt.text.Length == 0 || finished)
			return;

		timeOfLastChar += Time.deltaTime;

		if(timeOfLastChar > showNextCharDelay)
		{
			timeOfLastChar -= showNextCharDelay;
			txt.maxVisibleCharacters++;
		}

		if(txt.maxVisibleCharacters >= txt.text.Length)
		{
			finished = true;
			float alpha = 1;
			DOTween.To(() => alpha, x => alpha = x, 0, 1).OnUpdate(() =>
			{
				fadeArea.alpha = alpha;
			}).SetDelay(2 + ExtraLifeTime);
		}
	}
}
