using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _minimapIcon;
    private Color _defaultColor;
    private Color _enterColor;
    private Room _room;

    private void Start()
    {
        _room = GetComponent<Room>();
        _defaultColor = _minimapIcon.color;
        _enterColor = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _minimapIcon.color = _enterColor;
            _room.Enter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _minimapIcon.color = _defaultColor;
        }
    }
}
