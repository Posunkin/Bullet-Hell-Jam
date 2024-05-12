using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action<Enemy> OnEnemyDeath;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Material _damageMat;
    [SerializeField] private float _damageTime;
    private SpriteRenderer _sprite;
    private WaitForSeconds _damageWait;
    private Material _originalMat;
    private float _currentHealth;
    private Animator _anim;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _originalMat = _sprite.material;
        _damageWait = new WaitForSeconds(_damageTime);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            _anim.SetTrigger("Death");
        }
        else
        {
            StartCoroutine(DamageRoutine());
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator DamageRoutine()
    {
        _sprite.material = _damageMat;
        yield return _damageWait;
        _sprite.material = _originalMat;
    }
}
