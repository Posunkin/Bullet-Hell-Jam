using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IShootable
{
    [Header("Weapon parameters:")]
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _size;
    [SerializeField] private float _fireDelayPres;
    private WaitForSeconds _fireDelay;

    [Header("Particles parameters:")]
    [SerializeField] private ParticleSystem _system;
    [SerializeField] private MuzzlePoint _muzzlePoint;
    [SerializeField] private Material _bulletVisual;
    [SerializeField] private LayerMask _colliderLayer;

    private SpriteRenderer _sprite;
    private Animator _anim;
    private bool _shooting = false;
    private Camera _camera;

    private void Start()
    {
        _fireDelay = new WaitForSeconds(_fireDelayPres);
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _camera = Camera.main;
        Initialize();
    }

    private void Update()
    {
        RotateWeapon();
        RotateMuzzlePoint();
    }

    public void Shoot()
    {
        if (_shooting) return;
        EmitParticle();
    }

    private void RotateWeapon()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 screenToPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mouse.x < screenToPoint.x)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
        Vector2 pos = _camera.ScreenToWorldPoint(mouse);
        transform.LookAt(pos);
        Quaternion rot = transform.rotation;
        rot.x = 0;
        rot.y = 0;
        transform.rotation = rot;
    }

    private void RotateMuzzlePoint()
    {
        Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _system.transform.LookAt(pos);
    }

    private void EmitParticle()
    {
        _system.Play();
        _anim.SetTrigger("Shoot");
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
        _muzzlePoint.Init(_damage);
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

        var rend = system.GetComponent<ParticleSystemRenderer>();
        rend.material = _bulletVisual;

        var collision = system.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision2D;
        collision.lifetimeLoss = 1;
        collision.sendCollisionMessages = true;
        collision.collidesWith = _colliderLayer;
    }
}
