using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action<Enemy> OnEnemyDeath;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected Material _damageMat;
    [SerializeField] protected float _damageTime;
    protected SpriteRenderer _sprite;
    protected WaitForSeconds _damageWait;
    protected Material _originalMat;
    protected float _currentHealth;
    protected Animator _anim;

    protected void Start()
    {
        _currentHealth = _maxHealth;
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _originalMat = _sprite.material;
        _damageWait = new WaitForSeconds(_damageTime);
    }

    public virtual void TakeDamage(float damage)
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

    protected void Death()
    {
        Destroy(gameObject);
    }

    protected IEnumerator DamageRoutine()
    {
        _sprite.material = _damageMat;
        yield return _damageWait;
        _sprite.material = _originalMat;
    }
}
