using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour, IShootable
{
    [Header("Weapon parameters:")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected int _numOfBullets;
    [SerializeField] protected float _lifeTime;

    protected bool _canShoot = false;
    protected float _preDelay = 1f;
    protected WaitForSeconds _fireDelay;
    protected Transform _container;

    public virtual void Shoot()
    {
    }

    public abstract void StopShooting();
}
