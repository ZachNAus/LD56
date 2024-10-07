using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
	[SerializeField] float healthToGain = 1;

	[SerializeField] Vector3 rotate;

	public AudioClip pickupS;

	private void Update()
	{
		transform.Rotate(rotate * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<PlayerStats>(out var s))
		{
			if (s.CurrentHealth >= s.MaxHealth)
				return;

			s.TakeDamage(-healthToGain, transform, false);

			PlayerStats.instance.audioSource.PlayOneShot(pickupS);

			Destroy(gameObject);
		}
	}
}
