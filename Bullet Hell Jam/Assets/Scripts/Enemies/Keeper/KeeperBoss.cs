using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeeperBoss : MonoBehaviour, IDamageable
{
    public Action<KeeperBoss> OnKeeperDeath;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Material _damageMat;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _helathImage;
    private KeeperMaxAI _AI;
    private float _damageTime = 0.15f;
    private SpriteRenderer _sprite;
    private WaitForSeconds _damageWait;
    private Material _originalMat;
    private float _currentHealth;
    private bool _isDamageable;
    private bool _firstUp;
    private bool _lastUp;

    private void Awake()
    {
        _AI = GetComponent<KeeperMaxAI>();
        _currentHealth = _maxHealth;
        _sprite = GetComponent<SpriteRenderer>();
        _originalMat = _sprite.material;
        _damageWait = new WaitForSeconds(_damageTime);
        _healthBar.SetHealth(_maxHealth);
        _isDamageable = true;
        _firstUp = false;
        _lastUp = false;
    }

    public void TakeDamage(float damage)
    {
        if (_isDamageable)
        {
            _currentHealth -= damage;
            _healthBar.ChangeHealth(_currentHealth);
            if (_currentHealth <= 0)
            {
                OnKeeperDeath?.Invoke(this);
                Destroy(gameObject, 0.5f);
            }
            else if (_currentHealth < _maxHealth / 1.5f && !_firstUp)
            {
                _AI.UpIndex();
                _firstUp = true;
                StartCoroutine(DamageRoutine());
            }
            else if (_currentHealth < _maxHealth / 3 && !_lastUp)
            {
                _AI.UpIndex();
                _lastUp = true;
                StartCoroutine(DamageRoutine());
            }
            else
            {
                StartCoroutine(DamageRoutine());
            }
        }
    }

    private IEnumerator DamageRoutine()
    {
        _sprite.material = _damageMat;
        yield return _damageWait;
        _sprite.material = _originalMat;
    }

    public void Immortality(bool turnOn)
    {
        if (turnOn) 
        {
            _isDamageable = false;
            _helathImage.color = Color.gray;
        }
        else 
        {
            _isDamageable = true;
            _helathImage.color = Color.red;
        }
    }
}
