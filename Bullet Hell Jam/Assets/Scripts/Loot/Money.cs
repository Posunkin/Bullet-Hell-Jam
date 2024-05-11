using UnityEngine;

public class Money : Loot
{
    public int MoneyCount { get => _moneyCount; }
    [SerializeField,Range(1, 10)] private int _minCount;
    [SerializeField,Range(5, 25)] private int _maxCount;
    private int _moneyCount;
    
    private void Awake()
    {
        Type = LootType.Money;
        _moneyCount = Random.Range(_minCount, _maxCount);
    }
}
