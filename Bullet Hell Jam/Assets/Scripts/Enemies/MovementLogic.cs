using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Enemy _enemy;
    private Rigidbody2D _rb;
    private Movement _movement;
    private bool _isMoving;
    private float _moveTime = 2;
    private float _startMoveTime;

    void Start()
    {
        _isMoving = false;
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Movement(_enemy, _rb, _moveSpeed);
    }

    void Update()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            _movement.GetPatrolPosition();
            _startMoveTime = Time.time;
        }
        if (Time.time - _startMoveTime > _moveTime)
        {
            _isMoving = false;
        }
        _movement.MoveToPosition();
    }
}
