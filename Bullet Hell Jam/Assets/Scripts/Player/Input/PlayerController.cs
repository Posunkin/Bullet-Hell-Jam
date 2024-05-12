using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsDashing { get => _isDashing; }
    [Header("Movement parameters:")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCD;
    private bool _isDashing = false;
    private bool _dashOnCD = false;

    private WaitForSeconds _dashDurationSeconds;
    private WaitForSeconds _dashCDSeconds;

    private PlayerInput _playerInput;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private ActiveWeapon _weapon;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _weapon = GetComponentInChildren<ActiveWeapon>();
        _playerInput = new PlayerInput();
        _dashCDSeconds = new WaitForSeconds(_dashCD);
        _dashDurationSeconds = new WaitForSeconds(_dashDuration);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Movement.Dash.performed += _ => Dash();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        StopAllCoroutines();
    }

    private void Update()
    {
        GetInputs();
        ChangeFaceDirection();
    }

    private void FixedUpdate()
    {
        if (_isDashing) return;
        Move();
    }

    private void GetInputs()
    {
        _movement = _playerInput.Movement.Move.ReadValue<Vector2>();
        _anim.SetFloat("MoveX", _movement.x);
        _anim.SetFloat("MoveY", _movement.y);
    }

    private void ChangeFaceDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenToPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < screenToPoint.x)
        {
            _sprite.flipX = true;
            _weapon.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            _sprite.flipX = false;
        }
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }

    private void Dash()
    {
        if (!_dashOnCD)
        {
            _isDashing = true;
            _dashOnCD = true;
            _rb.velocity = new Vector2(_movement.x * _dashSpeed, _movement.y * _dashSpeed);
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        yield return _dashDurationSeconds;
        _isDashing = false;
        yield return _dashCDSeconds;
        _dashOnCD = false;
    }
}
