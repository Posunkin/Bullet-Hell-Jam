using Zenject;

public class EnemyPursuingWeapon : EnemyProjectileWeapon
{
    private PursuingBulletPool _pool;
    private PlayerStats _player;    

    [Inject]
    private void Construct(PursuingBulletPool pool, PlayerStats player)
    {
        _pool = pool;
        _player = player;
    }

    public override void Shoot()
    {
        if (_canShoot)
        {
            PursuingBullet bullet = _pool.GetPursuingBullet();
            bullet.transform.position = this.transform.position;
            bullet.Construct(_speed, _damage, _lifeTime, _player);
            StartCoroutine(ShootingRoutine());
        }
    }
}
