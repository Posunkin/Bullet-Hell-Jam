using System.Collections.Generic;
using UnityEngine;

public class PursuingBulletPool : EnemyBulletPool
{
    private  Queue<PursuingBullet> _bulletPool = new Queue<PursuingBullet>();

    protected override void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, this.transform);
            PursuingBullet bul = bullet.GetComponent<PursuingBullet>();
            bul.SetPool(this);
            bul.gameObject.SetActive(false);
            _bulletPool.Enqueue(bul);
        }
    }

    public PursuingBullet GetPursuingBullet()
    {
        if (_bulletPool.Count == 0)
        {
            GameObject bullet = Instantiate(_bulletPrefab, this.transform);
            PursuingBullet bul = bullet.GetComponent<PursuingBullet>();
            bul.SetPool(this);
            return bul;
        }

        PursuingBullet bulletFromPool = _bulletPool.Dequeue();
        bulletFromPool.gameObject.SetActive(true);
        return bulletFromPool;
    }

    public void ReturnPursuingBullet(PursuingBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
