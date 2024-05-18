using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public bool IsDashing { get => _isDashing; }
    public PlayerInput CurrentInput { get => _playerInput; }
    [Header("Movement parameters:")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCD;
    [SerializeField] private PlayerStatsSO _stats;
    [SerializeField] private ActiveWeapon _shotgun;
    [SerializeField] private ActiveWeapon[] _pistols;
    private bool _isDashing = false;
    private bool _dashOnCD = false;

    private WaitForSeconds _dashDurationSeconds;
    private WaitForSeconds _dashCDSeconds;

    private PlayerInput _playerInput;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _anim;
    private ActiveWeapon _weapon;
    private DialogueSystem _dialogueSystem;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem)
    {
        _dialogueSystem = dialogueSystem;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _weapon = GetComponentInChildren<ActiveWeapon>();
        _playerInput = new PlayerInput();
        _dashCD = _stats.DashCD;
        _dashCDSeconds = new WaitForSeconds(_dashCD);
        _dashDurationSeconds = new WaitForSeconds(_dashDuration);
        _dialogueSystem.DialogueStarted += DisableInput;
        _dialogueSystem.DialogueEnded += EnableInput;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        
    }

    private void Start()
    {
        _playerInput.Movement.Dash.performed += _ => Dash();
        _playerInput.Combat.ChangeWeapon.performed += _ => ChangeWeapon();
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

    #region Movement
    private void ChangeFaceDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenToPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < screenToPoint.x)
        {
            Quaternion rot = transform.rotation;
            rot.y = 180;
            transform.rotation = rot;
            _weapon.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Quaternion rot = transform.rotation;
            rot.y = 0;
            transform.rotation = rot;
            _weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }
    #endregion

    #region Dash
    private void Dash()
    {
        if (!_dashOnCD)
        {
            _isDashing = true;
            _dashOnCD = true;
            _anim.SetTrigger("Dash");
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
    #endregion

    #region For the dialogue
    private void DisableInput()
    {
        _playerInput.Disable();
    }

    private void EnableInput()
    {
        _playerInput.Enable();
    }
    #endregion

    private void ChangeWeapon()
    {
        if (_shotgun.gameObject.activeInHierarchy)
        {
            _shotgun.gameObject.SetActive(false);
            foreach (var pistol in _pistols)
            {
                pistol.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var pistol in _pistols)
            {
                pistol.gameObject.SetActive(false);
            }
            _shotgun.gameObject.SetActive(true);
        }
    }
}
