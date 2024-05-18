using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool HaveKey { get; private set; }

    [SerializeField] private float _maxHealth;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private bool _cantDie;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private Material _damageMat;
    [SerializeField] private PlayerStatsSO _stats;
    private SpriteRenderer _sprite;
    private Material _originalMat;
    private float _currentHealth;
    private bool _cantTakeDamage = false;
    private float _cantTakeDamageTime = 0.2f;
    private WaitForSeconds _cantTakeDamageWait;
    private PlayerController _playerController;
    private Wallet _wallet;

    private void Start()
    {
        HaveKey = false;
        _playerController = GetComponent<PlayerController>();
        _sprite = GetComponent<SpriteRenderer>();
        _maxHealth = _stats.MaxHealth;
        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_maxHealth);
        _cantTakeDamageWait = new WaitForSeconds(_cantTakeDamageTime);
        _originalMat = _sprite.material;
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
                    _playerUI.ArtifactMessage();
                    Destroy(other.gameObject);
                    break;
            }            
        }
    }

    public void TakeDamage(float damage)
    {
        if (_cantTakeDamage) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_cantDie) return;
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageRoutine());
            _healthBar.ChangeHealth(_currentHealth);
        }
    }

    private IEnumerator DamageRoutine()
    {
        _cantTakeDamage = true;
        _sprite.material = _damageMat;
        yield return _cantTakeDamageWait;
        _cantTakeDamage = false;
        _sprite.material = _originalMat;
    }
}
