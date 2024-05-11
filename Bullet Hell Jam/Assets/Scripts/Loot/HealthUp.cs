using UnityEngine;

public class HealthUp : Loot
{
    public float HealPercent { get => _percent; }
    [SerializeField, Range(0, 100)] private float _percent;

    private void Awake()
    {
        Type = LootType.HealthUp;
    }
}
