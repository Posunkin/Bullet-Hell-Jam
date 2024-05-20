using System.Collections;
using System;
using UnityEngine;

public class KeeperSphere : MonoBehaviour, IDamageable
{
    public Action OnSphereDeath;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Material _damageMat;
    private SphereAI _AI;
    private float _damageTime = 0.15f;
    private SpriteRenderer _sprite;
    private WaitForSeconds _damageWait;
    private Material _originalMat;
    private float _currentHealth;
    
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _originalMat = _sprite.material;
        _damageWait = new WaitForSeconds(_damageTime);
        _currentHealth = _maxHealth;
        _AI = GetComponent<SphereAI>();
    }

    public void Init()
    {
        _currentHealth = _maxHealth;
        _AI.Init();
    }

     public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            OnSphereDeath?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(DamageRoutine());
        }
    }

    private IEnumerator DamageRoutine()
    {
        _sprite.material = _damageMat;
        yield return _damageWait;
        _sprite.material = _originalMat;
    }
}
