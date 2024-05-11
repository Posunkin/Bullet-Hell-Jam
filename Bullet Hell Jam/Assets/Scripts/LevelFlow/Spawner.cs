using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemyPrefabs;
    private IInstantiator _instantiator;

    private int _enemyCount;
    private int _waveCount;
    private Room _currentRoom;
    private SpawnPoint[] _spawnPoints;

    private void Awake()
    {
        _enemyCount = 0;
        _waveCount = 0;
    }

    [Inject]
    private void Construct(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public void SpawnEnemies(Room room, SpawnPoint[] spawnPoints)
    {
        _spawnPoints = spawnPoints;
        _currentRoom = room;
        switch (_currentRoom.Type)
        {
            case RoomType.Basic:
                SpawnBasicEnemies();
                break;
            case RoomType.Challenge:
                SpawnChallengeEnemies();
                break;
        }

    }

    private void SpawnBasicEnemies()
    {
        foreach (var p in _spawnPoints)
        {
            Enemy go = _instantiator.InstantiatePrefabForComponent<Enemy>(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
            go.transform.position = p.gameObject.transform.position;
            go.OnEnemyDeath += EnemyDied;
            _enemyCount++;
        }   
    }

    private void SpawnChallengeEnemies()
    {
        _waveCount++;
        foreach (var p in _spawnPoints)
        {
            Enemy go = _instantiator.InstantiatePrefabForComponent<Enemy>(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
            go.transform.position = p.gameObject.transform.position;
            go.OnEnemyDeath += EnemyDiedChallenge;
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

    private void EnemyDiedChallenge(Enemy enemy)
    {
        enemy.OnEnemyDeath -= EnemyDiedChallenge;
        _enemyCount--;
        if (_enemyCount == 0)
        {
            if (_waveCount == 3) 
            {
                _currentRoom.OpenTheDoors();
                _waveCount = 0;
            }
            else
            {
                Invoke(nameof(SpawnChallengeEnemies), 1);
            }
        }
    }
}
