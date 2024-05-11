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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        if (_isDashing) return;
        Move();
    }

    private void GetInputs()
    {
        _movement = _playerInput.Movement.Move.ReadValue<Vector2>();
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
