using UnityEngine;

public class Money : Loot
{
    public int MoneyCount { get => _moneyCount; }
    [SerializeField,Range(1, 3)] private int _minCount;
    [SerializeField,Range(2, 6)] private int _maxCount;
    private int _moneyCount;
    
    private void Awake()
    {
        Type = LootType.Money;
        _moneyCount = Random.Range(_minCount, _maxCount);
    }
}
