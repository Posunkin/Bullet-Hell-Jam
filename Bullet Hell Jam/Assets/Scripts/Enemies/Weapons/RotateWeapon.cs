using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    
    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
