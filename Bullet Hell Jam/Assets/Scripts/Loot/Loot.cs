using UnityEngine;

public enum LootType
{
    HealthUp,
    Money,
    Key
}

public abstract class Loot : MonoBehaviour
{
    public LootType Type { get; protected set; } 

    [SerializeField] protected float _bobbingSpeed;
    [SerializeField] protected float _bobbingAmount;
    protected float _originalY;

    protected void Start()
    {
        _originalY = transform.position.y;
    }

    protected void Update()
    {
        float newY = _originalY + Mathf.Sin(Time.time * _bobbingSpeed) * _bobbingAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
