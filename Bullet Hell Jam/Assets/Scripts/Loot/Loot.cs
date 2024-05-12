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
}
