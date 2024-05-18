using UnityEngine;
using Zenject;

public enum RoomType
{
    Basic,
    Challenge,
    Boss,
    Quest
}

public class Room : MonoBehaviour
{
    public Transform RoomCenter { get => _roomCenter; }
    public RoomType Type { get => _type; }
    public Door DoorU { get => _doorU; }
    public Door DoorD { get => _doorD; }
    public Door DoorR { get => _doorR; }
    public Door DoorL { get => _doorL; }

    public bool HaveUpNeigh = false, HaveDownNeigh = false, HaveLeftNeigh = false, HaveRightNeigh = false;
    public Vector2Int Position;

    [SerializeField] private Door _doorU;
    [SerializeField] private Door _doorD;
    [SerializeField] private Door _doorR;
    [SerializeField] private Door _doorL;
    [SerializeField] private RoomType _type;
    [SerializeField] private bool _startedRoom;
    [SerializeField] private Transform _roomCenter;

    [SerializeField] private SpawnPoint[] _spawnPoints;

    private bool _roomVisited;
    private Spawner _spawner;

    private void Start()
    {
        if (Type == RoomType.Quest) _roomVisited = true;
        if (Type == RoomType.Boss) CloseTheDoors();
    }

    [Inject]
    private void Construct(Spawner spawner)
    {
        _spawner = spawner;
    }

    public void Enter()
    {
        SetCamera();
        if (_roomVisited || _startedRoom) return;
        CloseTheDoors();
        _roomVisited = true;
        _spawner.SpawnEnemies(this, _spawnPoints);
    }

    public void EnterBossRoom()
    {
        SetCamera();
        if (_roomVisited || _startedRoom) return;
        _roomVisited = true;
        _spawner.OpenPortal(this);
    }

    public void EnterQuestRoom()
    {
        CloseTheDoors();
        _spawner.SpawnQuestEnemy(this, _spawnPoints);
    }

    private void SetCamera()
    {
        Vector3 pos = _roomCenter.position;
        pos.z = - 10;
        Camera.main.transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (_type != RoomType.Boss) return;
        // if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
        // {
        //     Debug.Log(player);
        //     if (player.HaveKey)
        //     {
        //         Debug.Log("Player have key");
        //         OpenTheDoorsWithKey();
        //     }            
        // }
    }

    private void CloseTheDoors()
    {
        _doorD.gameObject.SetActive(true);
        _doorU.gameObject.SetActive(true);
        _doorR.gameObject.SetActive(true);
        _doorL.gameObject.SetActive(true);
    }

    public void OpenTheDoors()
    {
        _doorD.gameObject.SetActive(!HaveDownNeigh);
        _doorU.gameObject.SetActive(!HaveUpNeigh);
        _doorR.gameObject.SetActive(!HaveRightNeigh);
        _doorL.gameObject.SetActive(!HaveLeftNeigh);
    }

    private void OpenTheDoorsWithKey()
    {
        _doorD.gameObject.SetActive(false);
        _doorU.gameObject.SetActive(false);
        _doorR.gameObject.SetActive(false);
        _doorL.gameObject.SetActive(false);
    }
}
