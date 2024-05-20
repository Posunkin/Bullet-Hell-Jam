using UnityEngine;

public class SpinWeapon : KeeperWeapon
{
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _system;

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
