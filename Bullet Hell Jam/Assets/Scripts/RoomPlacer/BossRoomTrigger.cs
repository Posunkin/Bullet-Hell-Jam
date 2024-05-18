using UnityEngine;

public class BossRoomTrigger : RoomTrigger
{
    private PlayerStats _player;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            Debug.Log(player.HaveKey);
            _player = player;
            if (player.HaveKey)
            {
                _room.OpenTheDoors();
                _room.EnterBossRoom();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
