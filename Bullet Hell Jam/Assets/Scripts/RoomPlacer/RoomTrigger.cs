using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _minimapIcon;
    protected Color _defaultColor;
    protected Color _enterColor;
    protected Room _room;

    protected void Start()
    {
        _room = GetComponent<Room>();
        _defaultColor = _minimapIcon.color;
        _enterColor = Color.white;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _minimapIcon.color = _enterColor;
            _room.Enter();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _minimapIcon.color = _defaultColor;
        }
    }
}
