using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action<Enemy> OnEnemyDeath;
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    private Animator _anim;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            _anim.SetTrigger("Death");
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
