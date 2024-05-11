using UnityEngine;
using System.Collections;

public class EnemyProjectileWeapon : EnemyWeapon
{
    protected void Start()
    {
        _container = gameObject.transform.parent;
        StartCoroutine(PredelayRoutine());
        _fireDelay = new WaitForSeconds(_fireRate);
    }

    protected IEnumerator PredelayRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_preDelay);
        _canShoot = true;
    }

    public override void StopShooting()
    {
        Destroy(_container.gameObject);
    }

    protected IEnumerator ShootingRoutine()
    {
        _canShoot = false;
        yield return _fireDelay;
        _canShoot = true;
    }
}
