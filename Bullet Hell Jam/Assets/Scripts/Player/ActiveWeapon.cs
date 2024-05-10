using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    private Weapon _currentWeapon;
    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
        _currentWeapon = GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        _input.Combat.Shoot.started += _ => _currentWeapon.Shoot();
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
