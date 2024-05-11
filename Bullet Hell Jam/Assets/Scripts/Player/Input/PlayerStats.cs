using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool HaveKey { get; private set; }

    public void TakeDamage()
    {

    }

    private void OnTrigerEnter2D(Collider2D other)
    {
        Debug.Log("Collision with + " + other.gameObject.name);
    }
}
