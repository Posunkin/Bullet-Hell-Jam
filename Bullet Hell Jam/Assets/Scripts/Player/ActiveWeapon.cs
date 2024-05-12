using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _currentWeapon;
    private PlayerInput _input;
    private bool _shooting = false;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void Start()
    {
        _input.Combat.Shoot.started += _ => _shooting = true;
        _input.Combat.Shoot.canceled += _ => _shooting = false;
    }

    private void Update()
    {
        if (_shooting) _currentWeapon.Shoot();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
