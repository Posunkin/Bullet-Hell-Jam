using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemyPrefabs;
    private IInstantiator _instantiator;

    private int _enemyCount;
    private Room _currentRoom;

    private void Awake()
    {
        _enemyCount = 0;
    }

    [Inject]
    private void Construct(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public void SpawnEnemies(Room room, SpawnPoint[] spawnPoints)
    {
        _currentRoom = room;
        foreach (var p in spawnPoints)
        {
            Enemy go = _instantiator.InstantiatePrefabForComponent<Enemy>(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
            go.transform.position = p.gameObject.transform.position;
            go.OnEnemyDeath += EnemyDied;
            _enemyCount++;
        }
    }

    private void EnemyDied(Enemy enemy)
    {
        enemy.OnEnemyDeath -= EnemyDied;
        _enemyCount--;
        if (_enemyCount == 0)
        {
            _currentRoom.OpenTheDoors();
        }
    }
}
