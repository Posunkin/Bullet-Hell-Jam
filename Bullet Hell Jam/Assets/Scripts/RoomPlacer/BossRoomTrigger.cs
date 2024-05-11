using System.Collections;
using UnityEngine;

public class BossRoomTrigger : RoomTrigger
{
    private PlayerStats _player;
    private bool _playerEnteringRoom = false;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            _player = player;
            _playerEnteringRoom = true;
            StartCoroutine(RoomPathRoutine());
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        _playerEnteringRoom = false;
    }

    private IEnumerator RoomPathRoutine()
    {
        while (_playerEnteringRoom)
        {
            float dist = Vector2.Distance(_player.gameObject.transform.position, _room.RoomCenter.position);
            if (dist <= 12)
            {
                _room.EnterBossRoom();
                _playerEnteringRoom = false;
            }
            yield return null;
        }
    }
}
