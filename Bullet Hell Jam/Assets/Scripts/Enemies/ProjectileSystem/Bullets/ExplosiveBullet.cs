using System.Collections;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField] private GameObject _expVisual;

    private bool _hasDirection = false;
    private Vector2 _direction;
    private bool _canMove;
    private Transform _target;

    public void Construct(float speed, float damage, float lifeTime, PlayerStats _player)
    {   
        _expVisual.SetActive(false);
        _speed = speed;
        _damage = damage;
        _lifeTime = lifeTime;
        _target = _player.transform;
        _startTime = Time.time;
        _canMove = true;
    }

    protected override void Update()
    {
        if (_target == null) return;
        if (!_hasDirection) 
        {
            _direction = (_target.position - transform.position).normalized;
            _hasDirection = true;
        }
        if (_canMove) Move();
        if (Time.time - _startTime > _lifeTime) Explose();
    }


    protected override void Move()
    {
        Vector2 pos = transform.position;
        pos += _direction * _speed * Time.deltaTime;
        transform.position = pos;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        Explose();
    }

    private void Explose()
    {
        _canMove = false;
        Collider2D[] damage = Physics2D.OverlapCircleAll(this.transform.position, 5f);
        _expVisual.SetActive(true);
        foreach (Collider2D col in damage)
        {
            if (col.TryGetComponent<IDamageable>(out IDamageable dam))
            {
                dam.TakeDamage(_damage);
            }
        }
        StartCoroutine(ShootRoutine());
    }

    protected override void ReturnBullet()
    {
        _pool.ReturnExplosiveBullet(this);
    }

    private IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _pool.ReturnExplosiveBullet(this);
    }
}
