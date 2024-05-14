using System.Collections;
using UnityEngine;

public class GiantGoblinAI : MovementLogic
{
    [SerializeField] private EnemyParticleWeapon _weapon;
    private float _shootDelay = 3;
    private WaitForSeconds _shootWait;
    private bool _canShoot = true;

    protected override void Start()
    {
        _shootWait = new WaitForSeconds(_shootDelay);
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (_canShoot && Random.value > 0.6f)
        {
            _anim.SetTrigger("Attack");
            _canShoot = false;
        }
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
}
