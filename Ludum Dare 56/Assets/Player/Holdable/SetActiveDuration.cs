using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Sets a gameobject as active for a bit then turns it off.
/// </summary>
public class SetActiveDuration : MonoBehaviour
{
	public float duration;
	public float delay;
	public GameObject go;

	public Coroutine routine;

	public bool IsPlaying => routine != null;

	private void Awake()
	{
		if (go) go.SetActive(false);
	}

	private void OnDisable()
	{
		Stop();
	}

	public void Show()
	{
		Stop();
		if (go != null)
		{
			routine = StartCoroutine(Routine());
		}

		IEnumerator Routine()
		{
			if (delay > 0)
			{
				go.SetActive(false);
				yield return new WaitForSeconds(delay);
			}
			go.SetActive(true);
			yield return new WaitForSeconds(duration);
			go.SetActive(false);
			routine = null;
		}
	}

	public void Hide()
	{
		Stop();
		if (go != null)
		{
			go.SetActive(false);
		}
	}

	private void Stop()
	{
		if (routine != null)
		{
			StopCoroutine(routine);
			routine = null;
		}
	}
}