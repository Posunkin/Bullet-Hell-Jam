using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private HealthBar _healthBar;
    
    private void Awake()
    {
        _healthBar.SetHealth(_maxHealth);
    }

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.ChangeHealth(_currentHealth);
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            _anim.Play("Death");
        }
        else
        {
            StartCoroutine(DamageRoutine());
        }
    }
}
