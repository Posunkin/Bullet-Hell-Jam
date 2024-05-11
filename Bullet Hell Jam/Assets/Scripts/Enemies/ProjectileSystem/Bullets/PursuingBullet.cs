using UnityEngine;

public class PursuingBullet : Bullet
{
    private Transform _target;
    private Vector2 _direction;

    public void Construct(float speed, float damage, float lifeTime, PlayerStats _player)
    {   
        _speed = speed;
        _damage = damage;
        _lifeTime = lifeTime;
        _target = _player.transform;
        _startTime = Time.time;
    }

    protected override void Update()
    {
        _direction = (_target.position - transform.position).normalized;
        base.Update();
    }

    protected override void Move()
    {
        Vector2 pos = transform.position;
        pos += _direction * _speed * Time.deltaTime;
        transform.position = pos;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Triggered with " + other.gameObject.name);
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage();
        }
        ReturnBullet();
    }

    protected override void ReturnBullet()
    {
        _pool.ReturnBullet(this);
    }
}
