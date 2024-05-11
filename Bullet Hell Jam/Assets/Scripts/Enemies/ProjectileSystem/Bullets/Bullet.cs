using System.Collections;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _damage;
    protected float _lifeTime;
    protected float _startTime;
    protected EnemyBulletPool _pool;

    public void SetPool(EnemyBulletPool pool)
    {
        _pool = pool;
    }
    
    public void Construct(float speed, float damage, float lifeTime)
    {
        _speed = speed;
        _damage = damage;
        _lifeTime = lifeTime;
        _startTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time - _startTime > _lifeTime) ReturnBullet();
        Move();
    }

    protected abstract void Move();
    protected abstract void OnCollisionEnter2D(Collision2D collision);
    protected abstract void ReturnBullet();
}
