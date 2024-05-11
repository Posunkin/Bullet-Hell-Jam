using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool HaveKey { get; private set; }

    [SerializeField] private float _maxHealth;
    [SerializeField] private HealthBar _healthBar;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_maxHealth);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _healthBar.ChangeHealth(_currentHealth);
        }
    }
}
