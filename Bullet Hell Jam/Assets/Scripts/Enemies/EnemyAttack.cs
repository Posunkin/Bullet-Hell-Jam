using UnityEngine;
using Zenject;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyWeapon _weapon;
    [SerializeField] private bool _needToRotateWeapon;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SpriteRenderer _sprite;
    private PlayerStats _player;
    private bool _enemyAlive = true;
    private bool _weaponFlipped;

    private void Start()
    {
        _enemy.OnEnemyDeath += EnemyDied;
    }

    [Inject]
    private void Construct(PlayerStats player)
    {
        _player = player;
    }
    
    private void Update()
    {
        if (_needToRotateWeapon)
        {
            RotateWeapon();
        } 
        if (_enemyAlive) _weapon.Shoot();
    }

    private void RotateWeapon()
    {
        if (_player == null) return;
        Vector2 playerPos = _player.transform.position;
        if (transform.position.x > playerPos.x)
        {
            _weaponFlipped = true;
        }
        else
        {
            _weaponFlipped = false;
        }
        transform.LookAt(playerPos);
        Quaternion weaponRotation;
        if (_weaponFlipped)
        {
            weaponRotation = Quaternion.Euler(0, 180, -transform.rotation.eulerAngles.x);
            Quaternion rotation = _enemy.transform.rotation;
            rotation.y = 180;
            _enemy.transform.rotation = rotation;
        }
        else
        {
            weaponRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.x);
            Quaternion rotation = _enemy.transform.rotation;
            rotation.y = 0;
            _enemy.transform.rotation = rotation;
        }
        
        _weapon.transform.rotation = weaponRotation;
    }

    private void EnemyDied(Enemy enemy)
    {
        _needToRotateWeapon = false;
        transform.parent = null;
        _sprite.enabled = false;
        _enemyAlive = false;
        enemy.OnEnemyDeath -= EnemyDied;
        _weapon.StopShooting();
    }
}
