using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilLBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out IHasHealth h))
		{
			h.TakeDamage(100000, transform, false);
		}
	}
}
