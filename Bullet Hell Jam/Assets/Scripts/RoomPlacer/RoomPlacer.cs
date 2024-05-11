using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class RoomPlacer : MonoBehaviour
{
    [SerializeField] private Room[] _roomPrefabs;
    [SerializeField] private Room _bossRoomPrefab;
    [SerializeField] private Room _startingRoom;
    [SerializeField] private int _levelSize;

    private Room[,] _spawnedRooms;
    private IInstantiator _instantiator;

    private int _maxX, _maxY, _mid;
    private float _distanceToFarestRoom;
    private Vector2Int _farestRoomPos;
    private Room _farestRoom;

    [Inject]
    private void Construct(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    private IEnumerator Start()
    {
        _spawnedRooms = new Room[_levelSize, _levelSize];
        _mid = _levelSize / 2;
        _maxX = _spawnedRooms.GetLength(0) - 1;
        _maxY = _spawnedRooms.GetLength(1) - 1;
        _spawnedRooms[_mid, _mid] = _startingRoom;

        for (int i = 0; i < _levelSize; i++)
        {
            PlaceNewRoom();
            yield return new WaitForSeconds(0.5f);
        }
        // PlaceBossRoom();

        for (int x = 0; x < _spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedRooms.GetLength(1); y++)
            {
                if (_spawnedRooms[x, y] != null)
                {
                    ConnectToNeighbours(_spawnedRooms[x, y], _spawnedRooms[x,y].Position);
                }
            }
        }
    }

    private void PlaceNewRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < _spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedRooms.GetLength(1); y++)
            {
                if (_spawnedRooms[x, y] == null) continue;

                if (x > 0 && _spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < _maxX && _spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < _maxY && _spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }
        Room newRoom;
        Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
        newRoom = _instantiator.InstantiatePrefabForComponent<Room>(_roomPrefabs[Random.Range(0, _roomPrefabs.Length)]);
        newRoom.transform.position = new Vector3((position.x - _mid) * 42, (position.y - _mid) * 24, 0);
        newRoom.Position = position;
        float distance = Vector3.Distance(_startingRoom.transform.position, newRoom.transform.position); 
        if (distance > _distanceToFarestRoom)
        {
            _distanceToFarestRoom = distance;
            _farestRoom = newRoom;
            _farestRoomPos = position;
        }
        _spawnedRooms[position.x, position.y] = newRoom;
    }

    private void PlaceBossRoom()
    {
        Room bossRoom = Instantiate(_bossRoomPrefab);
        bossRoom.transform.position = _farestRoom.transform.position;
        _farestRoom.gameObject.SetActive(false);
        ConnectToNeighbours(bossRoom, _farestRoomPos);
    }

    private void ConnectToNeighbours(Room room, Vector2Int p)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorU != null && p.y < _maxY && _spawnedRooms[p.x, p.y + 1]?.DoorD != null) neighbours.Add(Vector2Int.up);
        if (room.DoorD != null && p.y > 0 && _spawnedRooms[p.x, p.y - 1]?.DoorU != null) neighbours.Add(Vector2Int.down);
        if (room.DoorR != null && p.x < _maxX && _spawnedRooms[p.x + 1, p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);
        if (room.DoorL != null && p.x > 0 && _spawnedRooms[p.x - 1, p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0 || room.Type == RoomType.Boss) return;

        foreach (var dir in neighbours)
        {
            Room selectedRoom = _spawnedRooms[p.x + dir.x, p.y + dir.y];
            if (dir == Vector2Int.up)
            {
                room.DoorU.gameObject.SetActive(false);
                room.HaveUpNeigh = true;
                selectedRoom.DoorD.gameObject.SetActive(false);
            }
            else if (dir == Vector2Int.down)
            {
                room.DoorD.gameObject.SetActive(false);
                room.HaveDownNeigh = true;
                selectedRoom.DoorU.gameObject.SetActive(false);
            }
            else if (dir == Vector2Int.right)
            {
                room.DoorR.gameObject.SetActive(false);
                room.HaveRightNeigh = true;
                selectedRoom.DoorL.gameObject.SetActive(false);
            }
            else if (dir == Vector2Int.left)
            {
                room.DoorL.gameObject.SetActive(false);
                room.HaveLeftNeigh = true;
                selectedRoom.DoorR.gameObject.SetActive(false);
            }
        }
    }
}
