using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
	private TextMeshProUGUI label;

	private void Awake()
	{
		label = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (PlayerStats.instance != null)
		{
			label.text = $"{PlayerStats.instance.CurrentHealth}/{PlayerStats.instance.MaxHealth}";
		}
	}
}