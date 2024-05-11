using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour, IShootable
{
    [Header("Weapon parameters:")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _size;
    [SerializeField] private float _fireRate;
    [SerializeField] private int _numOfBullets;

    [Header("Different shape parameters:")]
    [SerializeField] private bool _shapeEnable;

    [Header("Visual parameters:")]
    [SerializeField] private Material _bulletVisual;
    [SerializeField] private LayerMask _colliderLayer;

    private float _preDelay = 1;
    private bool _canShoot = false;
    private ParticleSystem _system;
    private WaitForSeconds _fireDelay;
    private Transform _container;

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

    public void Shoot()
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

    public void StopShooting()
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
