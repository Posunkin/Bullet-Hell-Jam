using UnityEngine;
using Zenject;

public class GoblinBossWeapon : EnemyProjectileWeapon
{
    private EnemyBulletPool _pool;
    private PlayerStats _player;

    [Inject]
    private void Construct(EnemyBulletPool pool, PlayerStats player)
    {
        _pool = pool;
        _player = player;
    }

    public override void Shoot()
    {
        if (_canShoot)
        {
            ExplosiveBullet bullet = _pool.GetExplosiveBullet();
            bullet.transform.position = this.transform.position;
            bullet.Construct(_speed, _damage, _lifeTime, _player);
            StartCoroutine(ShootingRoutine());
        }
    }
}
