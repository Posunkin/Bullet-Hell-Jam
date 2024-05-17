using System.Collections;
using UnityEngine;

public class GuardianAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private EnemyParticleWeapon _weapon;
    [SerializeField] private EnemyParticleWeapon _secondWeapon;
    private float _taskDelay = 2;
    private bool _firstAttack;
    private Movement _movement;
    private Rigidbody2D _rb;
    private bool _hasTask;
    private bool _isMoving;
    private Vector2 _direction;
    private Animator _anim;
    private WaitForSeconds _taskTime;
    private Coroutine _routine;
    private JumpAttack _jumpAttack;

    private void Start()
    {
        _taskTime = new WaitForSeconds(_taskDelay);
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _hasTask = false;
        _movement = new Movement(_rb, _moveSpeed);
        _jumpAttack = GetComponent<JumpAttack>();
    }
    
    protected void Update()
    {
        if (!_hasTask) SetTask();
    }

    private void SetTask()
    {
        if (Random.value > 0.7)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _hasTask = true;
            _routine = StartCoroutine(IdleTaskRoutine());
            return;
        }
        else if (Random.value > 0.7)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _hasTask = true;
            _routine = StartCoroutine(MovingTaskRoutine());
            return;
        }
        else if (Random.value > 0.5)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _hasTask = true;
            _routine = StartCoroutine(GroundAttackRoutine());
            return;
        }
        else if (Random.value > 0.5)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _hasTask = true;
            _routine = StartCoroutine(JumpAttackRoutine());
            return;
        }
        else
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _hasTask = true;
            _routine = StartCoroutine(SecondAttackRoutine());
            return;
        }

    }

    #region Tasks
    private IEnumerator IdleTaskRoutine()
    {
        _anim.SetBool("Walk", false);
        yield return _taskTime;
        _hasTask = false;
    }

    private IEnumerator MovingTaskRoutine()
    {
        _anim.SetBool("Walk", true);
        _direction = _movement.GetPatrolPosition();
        float start = Time.time;
        while (Time.time - start < _taskDelay)
        {
            _movement.MoveToPosition(_direction);
            yield return null;
        }
        _hasTask = false;
        _anim.SetBool("Walk", false);
    }

    private IEnumerator GroundAttackRoutine()
    {
        _firstAttack = true;
        _anim.SetTrigger("GroundAttack");
        _anim.SetBool("Attack", true);
        WaitForSeconds attackDelay = new WaitForSeconds(0.5f);
        int i = 0;
        while (i < 5)
        {
            if (Random.value > 0.3)
            {
                _anim.SetTrigger("RepeatAttack");
                i++;
                yield return attackDelay;
            }
            else break;
        }
        _anim.SetBool("Attack", false);
        yield return _taskTime;
        float time = Time.time;
        float cooldown = 0.5f;
        while (Time.time - time < cooldown)
        {
            yield return null;
        }
        _hasTask = false;
    }
    
    private IEnumerator JumpAttackRoutine()
    {
        _jumpAttack.Attack();
        while (_jumpAttack.Attacking)
        {
            yield return null;
        }
        yield return _taskTime;
        _hasTask = false;
    }

    private IEnumerator SecondAttackRoutine()
    {
        _firstAttack = false;
        _anim.SetTrigger("GroundAttack");
        _anim.SetBool("Attack", true);
        WaitForSeconds attackDelay = new WaitForSeconds(0.5f);
        int i = 0;
        while (i < 5)
        {
            if (Random.value > 0.3)
            {
                _anim.SetTrigger("RepeatAttack");
                i++;
                yield return attackDelay;
            }
            else break;
        }
        _anim.SetBool("Attack", false);
        yield return _taskTime;
        float time = Time.time;
        float cooldown = 0.5f;
        while (Time.time - time < cooldown)
        {
            yield return null;
        }
        _hasTask = false;
    }
    
    #endregion

    #region Actions
    private void GroundAttack()
    {
       if (_firstAttack) _weapon.Shoot();
       else _secondWeapon.Shoot();
    }
    #endregion

}
