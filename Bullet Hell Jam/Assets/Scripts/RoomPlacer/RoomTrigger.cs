using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Room _room;

    private void Start()
    {
        _room = GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _room.Enter();
        }
    }
}
