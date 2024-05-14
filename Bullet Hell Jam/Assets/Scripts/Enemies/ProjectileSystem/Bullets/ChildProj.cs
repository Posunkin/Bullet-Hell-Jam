using UnityEngine;

public class ChildProj : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(0.5f);
        }
        gameObject.SetActive(false);
    }
}
