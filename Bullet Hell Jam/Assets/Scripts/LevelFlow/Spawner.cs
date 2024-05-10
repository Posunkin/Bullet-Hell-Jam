using System.Drawing;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance; 
    [SerializeField] private Enemy[] _enemyPrefabs;

    private int _enemyCount;
    private Room _currentRoom;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            _enemyCount = 0;
        }
    }

    public void SpawnEnemies(Room room, SpawnPoint[] spawnPoints)
    {
        _currentRoom = room;
        foreach (var p in spawnPoints)
        {
            Enemy go = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
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
