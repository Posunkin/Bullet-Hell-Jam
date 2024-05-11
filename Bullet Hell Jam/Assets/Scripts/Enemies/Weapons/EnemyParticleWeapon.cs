using System.Collections;
using UnityEngine;

public class EnemyParticleWeapon : EnemyWeapon
{
    [Header("Weapon parameters:")]
    [SerializeField] private float _size;

    [Header("Different parameters:")]
    [SerializeField] private bool _shapeEnable;
    [SerializeField] private bool _projectilesVelocityEnabled;
    [SerializeField] private AnimationCurve _curve;

    [Header("Visual parameters:")]
    [SerializeField] private Material _bulletVisual;
    [SerializeField] private LayerMask _colliderLayer;

    private ParticleSystem _system;

    private void Start()
    {
        _container = gameObject.transform.parent;
        StartCoroutine(PredelayRoutine());
        _fireDelay = new WaitForSeconds(_fireRate);
        Initialize();
    }

    private void Initialize()
    {
        _system = GetComponent<ParticleSystem>();
        var mainModule = _system.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        mainModule.startLifetime = _lifeTime;
        mainModule.startSize = _size;
        mainModule.startSpeed = _speed;

        var form = _system.shape;
        form.enabled =  _shapeEnable;

        var velocity = _system.velocityOverLifetime;
        velocity.enabled = _projectilesVelocityEnabled;
        if (velocity.enabled) 
        {
            velocity.speedModifier = new ParticleSystem.MinMaxCurve(1, _curve);
        }

        var emission = _system.emission;
        emission.rateOverTime = 0;
        var burst = new ParticleSystem.Burst(0, _numOfBullets);
        emission.burstCount++;
        emission.SetBurst(emission.burstCount - 1, burst);

        var renderer = _system.GetComponent<Renderer>();
        renderer.material = _bulletVisual;

        var collision = _system.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision2D;
        collision.lifetimeLoss = 1;
        collision.sendCollisionMessages = true;
        collision.collidesWith = _colliderLayer;
    }

    public override void Shoot()
    {
        if (_canShoot)
        {
            _system.Play();
            StartCoroutine(ShootingRoutine());
        }
    }

    private IEnumerator PredelayRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_preDelay);
        _canShoot = true;
    }

    public override void StopShooting()
    {
        StopAllCoroutines();
        _canShoot = false;
        StartCoroutine(WaitingForParticles());
    }

    private IEnumerator WaitingForParticles()
    {
        while (_system.isPlaying)
        {
            yield return null;
        }
        Destroy(_container.gameObject);
    }

    private IEnumerator ShootingRoutine()
    {
        _canShoot = false;
        yield return _fireDelay;
        _canShoot = true;
    }
}
