using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    [SerializeField] private Loot[] _lootPrefabs;
    [SerializeField] private Loot[] _specialLootPrefabs;
    [SerializeField] private Loot _questRewardPrefab;
    [SerializeField] private Portal _portal;

    public void SpawnBaseReward(Transform room)
    {
        if (Random.value > 0.5f)
        {
            Loot go = Instantiate(_lootPrefabs[Random.Range(0, _lootPrefabs.Length)]);
            go.transform.position = room.position;
        }
    }

    public void SpawnQuestReward(Transform room)
    {
        Loot go = Instantiate(_questRewardPrefab);
        go.transform.position = room.position;
    }

    public void SpawnChallengeReward(Transform room)
    {
        bool rewardSpawned = false;
        for (int i = 0; i <= 3; i++)
        {
            if (!rewardSpawned || Random.value > 0.3f)
            {
                Loot go = Instantiate(_specialLootPrefabs[Random.Range(0, _specialLootPrefabs.Length)]);
                room.position = new Vector2(room.position.x + Random.Range(-5, 5), room.position.y + Random.Range(-5, 5));
                go.transform.position = room.position;
                rewardSpawned = true;
            }
        }
    }

    public void OpenPortal()
    {
        _portal.gameObject.SetActive(true);
    }
}
