using UnityEngine;
using Zenject;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyWeapon _weapon;
    [SerializeField] private bool _needToRotateWeapon;
    [SerializeField] private Enemy _enemy;
    private PlayerStats _player;
    private bool _enemyAlive = true;

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
        if (_needToRotateWeapon) RotateWeapon();
        if (_enemyAlive) _weapon.Shoot();
    }

    private void RotateWeapon()
    {
        Vector2 playerPos = _player.transform.position;
        transform.LookAt(playerPos);
    }

    private void EnemyDied(Enemy enemy)
    {
        transform.parent = null;
        _enemyAlive = false;
        enemy.OnEnemyDeath -= EnemyDied;
        _weapon.StopShooting();
    }
}
