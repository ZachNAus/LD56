using UnityEngine;

public class HoldableGun : Holdable
{
	public Bullet bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;

	public override void OnUse(bool down)
	{
		if (down)
		{
			var bullet = Instantiate(bulletPrefab);
			bullet.transform.position = bulletSpawn.position;
			bullet.holdable = this;
			bullet.velocity = player.transform.forward * bulletSpeed;
		}
	}
}