using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemyPrefabs;
    [SerializeField] private Enemy _questEnemyPrefab;
    [SerializeField] private Enemy _bossEnemyPrefab;
    private IInstantiator _instantiator;

    private int _enemyCount;
    private int _waveCount;
    private Room _currentRoom;
    private SpawnPoint[] _spawnPoints;
    private RewardSpawner _rewardSpawner;

    private void Awake()
    {
        _enemyCount = 0;
        _waveCount = 0;
        _rewardSpawner = GetComponent<RewardSpawner>();
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
            case RoomType.Boss:
                SpawnBoss();
                break;
        }
    }

    public void SpawnQuestEnemy(Room room, SpawnPoint[] point)
    {
        _currentRoom = room;
        Enemy go = _instantiator.InstantiatePrefabForComponent<Enemy>(_questEnemyPrefab);
        go.transform.position = point[0].transform.position;
        go.OnEnemyDeath += QuestEnemyDied;
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

    private void SpawnBoss()
    {
        Enemy go = _instantiator.InstantiatePrefabForComponent<Enemy>(_bossEnemyPrefab);
        go.transform.position = _spawnPoints[0].transform.position;
        go.OnEnemyDeath += BossDied;
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
            _rewardSpawner.SpawnBaseReward(_currentRoom.RoomCenter);
            _currentRoom.OpenTheDoors();
        }
    }

    private void BossDied(Enemy boss)
    {
        boss.OnEnemyDeath -= BossDied;
        _rewardSpawner.OpenPortal(_currentRoom.RoomCenter);
    }

    private void QuestEnemyDied(Enemy enemy)
    {
        enemy.OnEnemyDeath -= QuestEnemyDied;
        _rewardSpawner.SpawnQuestReward(_currentRoom.RoomCenter);
        _currentRoom.OpenTheDoors();
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
                _rewardSpawner.SpawnChallengeReward(_currentRoom.RoomCenter);
                _waveCount = 0;
            }
            else
            {
                Invoke(nameof(SpawnChallengeEnemies), 1);
            }
        }
    }
}
