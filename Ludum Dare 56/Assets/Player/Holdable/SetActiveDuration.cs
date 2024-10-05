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

	private void Awake()
	{
		if (go) go.SetActive(false);
	}

	public void Show()
	{
		StopRoutine();
		if (go != null)
		{
			routine = GameObjectUtilities.SetActive(go, duration, delay, this);
		}
	}

	public void Hide()
	{
		StopRoutine();
		if (go != null)
		{
			go.SetActive(false);
		}
	}

	private void StopRoutine()
	{
		if (routine != null)
		{
			StopCoroutine(routine);
			routine = null;
		}
	}
}