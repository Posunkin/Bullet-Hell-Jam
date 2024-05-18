using UnityEngine;

public class Money : Loot
{
    public int MoneyCount { get => _moneyCount; }
    [SerializeField] private int _moneyCount;
    
    private void Awake()
    {
        Type = LootType.Money;
    }
}
