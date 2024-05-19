using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _currentWeapon;
    private PlayerController _playerController;
    private PlayerInput _input;
    private bool _shooting = false;
    private InputAction _startShoot;

    // private void Awake()
    // {
    //     _playerController = GetComponentInParent<PlayerController>();
    // }

    // private void OnEnable()
    // {
    //     _shooting = false;
    //     if (_startShoot != null) 
    //     {
    //         _startShoot.Enable();
    //         _startShoot.Reset();
    //     }
    // }

    // private void OnDisable()
    // {
    //     _startShoot.Disable();
    // }

    private void Start()
    {
        // _input = _playerController.CurrentInput;
        // _startShoot = _input.Combat.Shoot;
        // _startShoot.started += Shoot;
        // _startShoot.canceled += StopShoot;
    }

    private void Update()
    {
        // if (_shooting) _currentWeapon.Shoot();
        if (Input.GetMouseButton(0)) _currentWeapon.Shoot();
    }

    // private void Shoot(InputAction.CallbackContext context)
    // {
    //     _shooting = true;
    // }

    // private void StopShoot(InputAction.CallbackContext context)
    // {
    //     _shooting = false;
    // }
}
