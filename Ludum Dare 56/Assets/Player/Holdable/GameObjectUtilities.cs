using System.Collections;
using UnityEngine;

public static class GameObjectUtilities
{
	/// <summary>
	/// Turns a gameobject on for a bit, and then disables it.
	/// </summary>
	/// <param name="go"></param>
	/// <param name="durationSeconds"></param>
	/// <param name="afterDelay"></param>
	/// <param name="runCoroutineOn"></param>
	/// <returns></returns>
	public static Coroutine SetActive(GameObject go, float durationSeconds, float afterDelay, MonoBehaviour runCoroutineOn)
	{
		return runCoroutineOn.StartCoroutine(Routine());
		IEnumerator Routine()
		{
			if (afterDelay > 0)
			{
				go.SetActive(false);
				yield return new WaitForSeconds(afterDelay);
			}
			go.SetActive(true);
			yield return new WaitForSeconds(durationSeconds);
			go.SetActive(false);
		}
	}
}