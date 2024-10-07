using DG.Tweening;
using UnityEngine;

public class PlayerStats : BaseHealth
{
    public static PlayerStats instance;

	public AudioSource audioSource;

	public AudioClip onBlock;

	protected override void Awake()
	{
		base.Awake();
		instance = this;
	}

	public override void TakeDamage(float damage, Transform source, bool doKnockback)
	{
		float res = Player.instance.TakeDamage(damage, source);

		if(res > 0)
		{
			//UnityEngine.Camera.main.DOShakePosition(0.5f);

			healthBar.transform.DOShakePosition(0.5f, 50);
		}
		else
		{
			if(onBlock && res == 0)
				GetComponent<AudioSource>().PlayOneShot(onBlock);
		}

		base.TakeDamage(res, source, doKnockback);
	}

	public override void Die()
	{
		Player.instance.Die();

		Cursor.lockState = CursorLockMode.None;

		base.Die();
	}
}