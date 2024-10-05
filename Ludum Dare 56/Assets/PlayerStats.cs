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
		float res = Player.instance.TakeDamage(damage);
		base.TakeDamage(res, source, doKnockback);
	}

	public override void Die()
	{
		Player.instance.Die();
		base.Die();
	}
}