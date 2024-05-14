using System.Collections;
using UnityEngine;
using Zenject;

public class GiantGoblinAI : MovementLogic
{
    [SerializeField] private EnemyParticleWeapon _weapon;
    private float _shootDelay = 3;
    private WaitForSeconds _shootWait;
    private bool _canShoot = true;
    private bool _attackAnimPlaying = false;
    private Transform _player;

    [Inject]
    private void Construct(PlayerStats playerStats)
    {
        _player = playerStats.gameObject.transform;
    }

    protected override void Start()
    {
        _shootWait = new WaitForSeconds(_shootDelay);
        base.Start();
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

    private void Flip()
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

    private void Shoot()
    {
        _weapon.Shoot();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return _shootWait;
        _canShoot = true;
    }

    private void AttackAnimEnded()
    {
        _attackAnimPlaying = false;
    }
}
