using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IShootable
{
    [Header("Weapon parameters:")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _size;
    [SerializeField] private float _fireDelayPres;
    private WaitForSeconds _fireDelay;

    [Header("Particles parameters:")]
    [SerializeField] private LayerMask colliderLayer;

    private ParticleSystem _system;
    private bool _shooting = false;
    private Camera _camera;

    private void Start()
    {
        _fireDelay = new WaitForSeconds(_fireDelayPres);
        _system = GetComponent<ParticleSystem>();
        _camera = Camera.main;
        Initialize();
    }

    private void Update()
    {
        RotateWeapon();
    }

    public void Shoot()
    {
        if (_shooting) return;
        EmitParticle();
    }

    private void RotateWeapon()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(position: Input.mousePosition);
        transform.LookAt(mousePos);
    }

    private void EmitParticle()
    {
        _system.Play();
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        _shooting = true;
        yield return _fireDelay;
        _shooting = false;
        yield break;
    }


    private void Initialize()
    {
        ChangeParameters(_system);
        _system.Stop();
    }

    private void ChangeParameters(ParticleSystem system)
    {
        var mainModule = system.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        mainModule.startLifetime = _lifeTime;
        mainModule.startSize = _size;
        mainModule.startSpeed = _speed;

        var form = system.shape;
        form.enabled = false;

        var rend = system.GetComponent<ParticleSystemRenderer>();
        rend.renderMode = ParticleSystemRenderMode.Mesh;

        var collision = system.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision2D;
        collision.lifetimeLoss = 1;
        collision.sendCollisionMessages = true;
        collision.collidesWith = colliderLayer;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage();
        }
    }
}
