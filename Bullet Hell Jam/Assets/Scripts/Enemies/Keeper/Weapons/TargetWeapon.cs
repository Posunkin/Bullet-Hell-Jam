using System.Collections;
using UnityEngine;
using Zenject;

public class TargetWeapon : KeeperWeapon
{
    private PlayerStats _player;
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _system;

    [Inject]
    private void Construct(PlayerStats player)
    {
        _player = player;
    }

    private void Update()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        if (_player == null) return;
        Vector2 playerPos = _player.transform.position;
        transform.LookAt(playerPos);
    }

    public override void StartShooting()
    {
        _system.Play();
    }

    public override void StopShooting()
    {
        _system.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(_damage);
        }
    }
}
