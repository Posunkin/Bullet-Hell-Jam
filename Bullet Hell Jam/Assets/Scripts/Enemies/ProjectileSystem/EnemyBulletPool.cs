using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    [SerializeField] protected GameObject _bulletPrefab;

    protected int _poolSize = 20;
    private Queue<Bullet> _bulletPool = new Queue<Bullet>();

    protected void Awake()
    {
        InitializePool();
    } 

    protected virtual void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, this.transform);
            Bullet bul = bullet.GetComponent<Bullet>();
            bul.SetPool(this);
            bul.gameObject.SetActive(false);
            _bulletPool.Enqueue(bul);
        }
    }

    public Bullet GetBullet()
    {
        if (_bulletPool.Count == 0)
        {
            GameObject bullet = Instantiate(_bulletPrefab, this.transform);
            Bullet bul = bullet.GetComponent<Bullet>();
            bul.SetPool(this);
            return bul;
        }

        Bullet bulletFromPool = _bulletPool.Dequeue();
        bulletFromPool.gameObject.SetActive(true);
        return bulletFromPool;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
