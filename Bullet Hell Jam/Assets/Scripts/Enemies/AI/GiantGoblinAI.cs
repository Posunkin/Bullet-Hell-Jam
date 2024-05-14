using System.Collections;
using UnityEngine;
using Zenject;

public class GiantGoblinAI : MovementLogic
{
    [SerializeField] protected EnemyParticleWeapon _weapon;
    private float _shootDelay = 3;
    protected WaitForSeconds _shootWait;
    protected bool _canShoot = true;
    protected bool _attackAnimPlaying = false;
    protected Transform _player;

    [Inject]
    protected void Construct(PlayerStats playerStats)
    {
        _player = playerStats.gameObject.transform;
    }

    protected override void Start()
    {
        _shootWait = new WaitForSeconds(_shootDelay);
        base.Start();
        _enemy.OnEnemyDeath += Death;
    }
    
    protected override void Update()
    {
        if (!_hasTask) SetTask();
        if (_isMoving)
        {
            _movement.MoveToPosition(_direction);
            _anim.SetBool("Walk", _isMoving);
        }
        Flip();
        if (_canShoot && Random.value > 0.6f)
        {
            _anim.SetTrigger("Attack");
            _movement.Stay();
            _canShoot = false;
            _attackAnimPlaying = true;
        }
    }

    protected void Flip()
    {
        Quaternion rotate;
        if (_player.position.x < transform.position.x)
        {
            rotate = transform.rotation;
            rotate.y = 180;
        }
        else
        {
            rotate = transform.rotation;
            rotate.y = 0;
        }
        transform.rotation = rotate;
    }

    protected void Shoot()
    {
        _weapon.Shoot();
        StartCoroutine(AttackRoutine());
    }

    protected IEnumerator AttackRoutine()
    {
        yield return _shootWait;
        _canShoot = true;
    }

    protected void AttackAnimEnded()
    {
        _attackAnimPlaying = false;
    }

    protected void Death(Enemy enemy)
    {
        _enemy.OnEnemyDeath -= Death;
        _weapon.StopShooting();
    }
}
