using UnityEngine;
using Zenject;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool HaveKey { get; private set; }

    [SerializeField] private float _maxHealth;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private bool _cantDie;
    private float _currentHealth;
    private PlayerController _playerController;
    private Wallet _wallet;

    private void Start()
    {
        HaveKey = false;
        _playerController = GetComponent<PlayerController>();
        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_maxHealth);
    }

    [Inject]
    private void Construct(Wallet wallet)
    {
        _wallet = wallet;
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
                case LootType.Money:
                    Money money = (Money)loot;
                    _wallet.AddMoney(money.MoneyCount);
                    Destroy(other.gameObject);
                    break;
                case LootType.Key:
                    HaveKey = true;
                    Destroy(other.gameObject);
                    break;
            }            
        }
    }

    public void TakeDamage(float damage)
    {
        if (_playerController.IsDashing || _cantDie) return;
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
