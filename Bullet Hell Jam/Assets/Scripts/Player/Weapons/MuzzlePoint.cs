using UnityEngine;

public class MuzzlePoint : MonoBehaviour
{
    private ParticleSystem _system;
    private float _damage;

    public void Init(float damage)
    {
        _damage = damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(_damage);
        }
    }
}
