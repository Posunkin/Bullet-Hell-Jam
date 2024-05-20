using System.Collections;
using UnityEngine;

public class SphereAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private KeeperWeapon[] _weapons;
    private KeeperWeapon _currentWeapon;
    private float _moveTaskDelay = 2;
    private float _attackTaskDelay = 3;
    private float _attackRecharge = 5;
    private Rigidbody2D _rb;
    private Movement _movement;
    private Vector2 _direction;
    private bool _isMoving;
    private bool _hasMoveTask;
    private bool _hasAttackTask;
    private WaitForSeconds _moveTaskTime; 
    private WaitForSeconds _attackTaskTime;
    private WaitForSeconds _attackRechargeTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Movement(_rb, _moveSpeed);
        _hasAttackTask = false;
        _hasMoveTask = false;
        _isMoving = false;
        _moveTaskTime = new WaitForSeconds(_moveTaskDelay);
        _attackTaskTime = new WaitForSeconds(_attackTaskDelay);
        _attackRechargeTime = new WaitForSeconds(_attackRecharge);
    }

    private void Update()
    {
        if (!_hasMoveTask) SetMoveTask();
        if (!_hasAttackTask) SetAttackTask();
        if (_isMoving)
        {
            _movement.MoveToPosition(_direction);
        }
    }

    #region Move Tasks
    private void SetMoveTask()
    {
        if (Random.value > 0.5)
        {
            _hasMoveTask = true;
            _isMoving = true;
            _direction = _movement.GetPatrolPosition();
            StartCoroutine(MovementRoutine());
        }
        else
        {
            _hasMoveTask = true;
            _isMoving = false;
            StartCoroutine(MovementRoutine());
        }
    }

    private IEnumerator MovementRoutine()
    {
        yield return _moveTaskTime;
        _isMoving = false;
        _hasMoveTask = false;
    }
    #endregion

    #region  Attack Tasks
    private void SetAttackTask()
    {
        _currentWeapon = _weapons[Random.Range(0, _weapons.Length)];
        _hasAttackTask = true;
        _currentWeapon.StartShooting();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return _attackTaskTime;
        _currentWeapon.StopShooting();
        yield return _attackRechargeTime;
        _hasAttackTask = false;
    }
    #endregion

    public void Init()
    {
        StopAllCoroutines();
        _hasMoveTask = false;
        _hasAttackTask = false;
    }
}
