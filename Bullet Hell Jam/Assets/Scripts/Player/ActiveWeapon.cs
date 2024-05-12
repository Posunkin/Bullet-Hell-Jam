using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _currentWeapon;
    private PlayerController _playerController;
    private PlayerInput _input;
    private bool _shooting = false;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        _input = _playerController.CurrentInput;
        _input.Combat.Shoot.started += _ => _shooting = true;
        _input.Combat.Shoot.canceled += _ => _shooting = false;
    }

    private void Update()
    {
        if (_shooting) _currentWeapon.Shoot();
    }
}
