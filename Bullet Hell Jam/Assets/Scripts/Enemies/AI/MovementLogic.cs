using System.Collections;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    protected float _taskDelay = 2;
    protected Rigidbody2D _rb;
    protected Movement _movement;
    protected bool _hasTask;
    protected bool _isMoving;
    protected Vector2 _direction;
    protected Animator _anim;
    protected WaitForSeconds _taskTime;

    protected virtual void Start()
    {
        _taskTime = new WaitForSeconds(_taskDelay);
        _anim = GetComponent<Animator>();
        _hasTask = false;
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Movement( _rb, _moveSpeed);
    }

    protected virtual void Update()
    {
        if (!_hasTask) SetTask();
        if (_isMoving)
        {
            _movement.MoveToPosition(_direction);
            _anim.SetBool("Walk", _isMoving);
        }
    }

    protected virtual void SetTask()
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

    protected IEnumerator MovementRoutine()
    {
        yield return _taskTime;
        _hasTask = false;
        _isMoving = false;
    }
}
