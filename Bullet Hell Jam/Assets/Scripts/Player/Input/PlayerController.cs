using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private Material _dashMat;
    [SerializeField] private TrailRenderer _dashRend;
    private bool _isDashing = false;
    private bool _dashOnCD = false;
    private bool _shadowDash;
    private SpriteRenderer _sprite;
    private Material _material;


    private WaitForSeconds _dashDurationSeconds;
    private WaitForSeconds _dashCDSeconds;

    private PlayerInput _playerInput;
    private InputAction _dashAction;
    private InputAction _changeWeaponAction;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _anim;
    private ActiveWeapon _weapon;
    private DialogueSystem _dialogueSystem;
    private StoryFlowHandler _storyFlowHandler;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, StoryFlowHandler storyFlowHandler)
    {
        _dialogueSystem = dialogueSystem;
        _storyFlowHandler = storyFlowHandler;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _weapon = GetComponentInChildren<ActiveWeapon>();
        _sprite = GetComponent<SpriteRenderer>();
        _material = _sprite.material;
        _playerInput = new PlayerInput();
        if (_stats.DashSceneRecieve < _storyFlowHandler.CurrentScene) _shadowDash = _stats.ShadowDash;
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
        _dashAction = _playerInput.Movement.Dash;
        _dashAction.performed += Dash;
        _changeWeaponAction = _playerInput.Combat.ChangeWeapon;
        _changeWeaponAction.performed += ChangeWeapon;
        _dashAction.Enable();
        _changeWeaponAction.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _dashAction.Disable();
        _changeWeaponAction.Disable();
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
    private void Dash(InputAction.CallbackContext context)
    {
        if (!_dashOnCD)
        {
            _isDashing = true;
            _dashOnCD = true;
            _anim.SetTrigger("Dash");
            if (_shadowDash)
            {
                _sprite.material = _dashMat;
                _dashRend.emitting = true;
            } 
            _rb.velocity = new Vector2(_movement.x * _dashSpeed, _movement.y * _dashSpeed);
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        yield return _dashDurationSeconds;
        _isDashing = false;
        if (_shadowDash) 
        {
            _sprite.material = _material;
            _dashRend.emitting = false;
        }
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

    private void ChangeWeapon(InputAction.CallbackContext context)
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

    private void OnDestroy()
    {
        _playerInput.Disable();
        _dashAction.performed -= Dash;
        _dashAction.Disable();
        _changeWeaponAction.performed -= ChangeWeapon;
        _changeWeaponAction.Disable();
    }
}
