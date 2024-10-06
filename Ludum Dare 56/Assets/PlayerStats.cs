using DG.Tweening;

public class PlayerStats : BaseHealth
{
    public static PlayerStats instance;

	protected override void Awake()
	{
		base.Awake();
		instance = this;
	}

	public override void TakeDamage(float damage, UnityEngine.Transform source, bool doKnockback)
	{
		float res = Player.instance.TakeDamage(damage, source);

		if(res > 0)
		{
			//UnityEngine.Camera.main.DOShakePosition(0.5f);

			healthBar.transform.DOShakePosition(0.5f, 50);
		}

		base.TakeDamage(res, source, doKnockback);
	}

	public override void Die()
	{
		Player.instance.Die();
		base.Die();
	}
}