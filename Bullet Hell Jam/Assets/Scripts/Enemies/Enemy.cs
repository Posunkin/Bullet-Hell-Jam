using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action<Enemy> OnEnemyDeath;
    private float hp = 2;

    public void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
