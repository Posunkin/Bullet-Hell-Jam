using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected GameObject _explosiveBulletPrefab;

    protected int _poolSize = 10;
    private Queue<Bullet> _bulletPool = new Queue<Bullet>();
    private Queue<ExplosiveBullet> _explosivePool = new Queue<ExplosiveBullet>();

    protected void Awake()
    {
        InitializePool();
        InitializeExplosivePool();
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

    private void InitializeExplosivePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_explosiveBulletPrefab, this.transform);
            ExplosiveBullet bul = bullet.GetComponent<ExplosiveBullet>();
            bul.SetPool(this);
            bul.gameObject.SetActive(false);
            _explosivePool.Enqueue(bul);
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

    public ExplosiveBullet GetExplosiveBullet()
    {
        if (_explosivePool.Count == 0)
        {
            GameObject bullet = Instantiate(_explosiveBulletPrefab, this.transform);
            ExplosiveBullet bul = bullet.GetComponent<ExplosiveBullet>();
            bul.SetPool(this);
            return bul;
        }

        ExplosiveBullet bulletFromPool = _explosivePool.Dequeue();
        bulletFromPool.gameObject.SetActive(true);
        return bulletFromPool;
    }

    public void ReturnExplosiveBullet(ExplosiveBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
