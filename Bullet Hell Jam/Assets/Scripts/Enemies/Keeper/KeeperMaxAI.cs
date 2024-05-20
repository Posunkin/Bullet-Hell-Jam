using System.Collections;
using UnityEngine;

public class KeeperMaxAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private KeeperWeapon[] _weapons;
    [SerializeField] private KeeperSphere _sphere;
    private Vector2 _center = Vector2.zero;
    private KeeperWeapon _currentWeapon;
    private float _moveTaskDelay = 2;
    private float _attackTaskDelay = 5;
    private float _attackRecharge = 3f;
    private Rigidbody2D _rb;
    private Movement _movement;
    private bool _isMoving;
    private bool _hasMoveTask;
    private bool _hasAttackTask;
    private Vector2 _direction;
    private Animator _anim;
    private WaitForSeconds _moveTaskTime;
    private WaitForSeconds _attackTaskTime;
    private WaitForSeconds _attackRechargeTime;
    private int _attackMaxIndex;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _attackMaxIndex = 3;
        _rb = GetComponent<Rigidbody2D>();
        _movement = new Movement(_rb, _moveSpeed);
        _hasAttackTask = false;
        _hasMoveTask = false;
        _isMoving = false;
        _moveTaskTime = new WaitForSeconds(_moveTaskDelay);
        _attackTaskTime = new WaitForSeconds(_attackTaskDelay);
        _attackRechargeTime = new WaitForSeconds(_attackRecharge);
        _sphere.OnSphereDeath += SphereDead;
    }

    private void OnDisable()
    {
        _sphere.OnSphereDeath -= SphereDead;
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
            if (transform.position.x > _center.x && _direction.x > 0 ||
            transform.position.x < _center.x && _direction.x < 0) _direction.x *= -1;
            if (_direction.y > _center.y && _direction.y > 0 ||
            transform.position.y < _center.y && _direction.y < 0) _direction.y *= -1;
            _anim.SetBool("Walk", true);
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
        _anim.SetBool("Walk", false);
        _isMoving = false;
        _hasMoveTask = false;
    }
    #endregion

    #region  Attack Tasks
    private void SetAttackTask()
    {
        _currentWeapon = _weapons[Random.Range(0, _attackMaxIndex)];
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

    public void UpIndex()
    {
        if (_attackMaxIndex < _weapons.Length)
        {
            _attackMaxIndex++;
        }
    }

    private void SphereDead()
    {
        StopAllCoroutines();
        _hasMoveTask = true;
        _hasAttackTask = true;
        _isMoving = false;
        _currentWeapon.StopShooting();
        _anim.SetBool("Walk", false);
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        yield return _attackTaskTime;
        _hasMoveTask = false;
        _hasAttackTask = false;
    }
}
