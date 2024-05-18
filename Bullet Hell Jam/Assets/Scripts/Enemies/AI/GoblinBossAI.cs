using UnityEngine;

public class GoblinBossAI : GiantGoblinAI
{
    [SerializeField] private EnemyProjectileWeapon _gun;
    private float _shootDelay = 2;
    private Boss _boss;

    protected override void Start()
    {
        base.Start();
        _boss = GetComponent<Boss>();
        _boss.OnEnemyDeath += Death;
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
            if (Random.value > 0.8f)
            {
                _anim.SetTrigger("Attack");
                _movement.Stay();
                _canShoot = false;
                _attackAnimPlaying = true;
            }
            else
            {
                _gun.Shoot();
                _movement.Stay();
                _canShoot = false;
                StartCoroutine(AttackRoutine());
            }
        }
    }

    protected override void Death(Enemy enemy)
    {
        _boss.OnEnemyDeath -= Death;
        _weapon.StopShooting();
    }
}
