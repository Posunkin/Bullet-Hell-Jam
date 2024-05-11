using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action<Enemy> OnEnemyDeath;
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject, 0.2f);
        }
    }
}
