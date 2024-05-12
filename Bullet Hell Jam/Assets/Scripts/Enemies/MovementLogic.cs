using System.Collections;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float _taskDelay = 2;
    private Enemy _enemy;
    private Rigidbody2D _rb;
    private Movement _movement;
    private bool _hasTask;
    private bool _isMoving;
    private Vector2 _direction;
    private Animator _anim;
    private WaitForSeconds _taskTime;

    void Start()
    {
        _taskTime = new WaitForSeconds(_taskDelay);
        _anim = GetComponent<Animator>();
        _hasTask = false;
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Movement(_enemy, _rb, _moveSpeed);
    }

    void Update()
    {
        if (!_hasTask)
        {
            if (Random.value > 0.5)
            {
                Debug.Log("IDLE");
                _isMoving = false;
                _hasTask = true;
                _anim.SetBool("Walk", _isMoving);                                
                StartCoroutine(MovementRoutine());
            }
            else
            {
                _isMoving = true;
                _hasTask = true;
                _direction = _movement.GetPatrolPosition();
                StartCoroutine(MovementRoutine());
            }
        }
        if (_isMoving)
        {   
            _movement.MoveToPosition(_direction);
            _anim.SetBool("Walk", _isMoving);
        }
    }

    private IEnumerator MovementRoutine()
    {
        yield return _taskTime;
        _hasTask = false;
        _isMoving = false;
    }
}
