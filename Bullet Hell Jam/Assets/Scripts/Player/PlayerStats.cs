using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool HaveKey { get; private set; }

    [SerializeField] private float _maxHealth;
    [SerializeField] private HealthBar _healthBar;
    private float _currentHealth;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Loot>(out Loot loot))
        {
            switch (loot.Type)
            {
                case LootType.HealthUp:
                    if (_currentHealth != _maxHealth)
                    {
                        HealthUp healthUp = (HealthUp)loot;
                        Debug.Log(healthUp.HealPercent);
                        _currentHealth += _maxHealth * (healthUp.HealPercent / 100);
                        _healthBar.ChangeHealth(_currentHealth);
                        Destroy(other.gameObject);
                    }
                    break;
            }            
        }
    }

    public void TakeDamage(float damage)
    {
        if (_playerController.IsDashing) return;
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
