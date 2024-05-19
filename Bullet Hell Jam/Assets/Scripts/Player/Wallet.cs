using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Sum { get => _sum; }

    [SerializeField] private TextMeshProUGUI _sumVisual;
    [SerializeField] private PlayerStats _player;

    private int _sum;
    private int _savedMoney;

    private void Start()
    {
        _sum = _savedMoney;
        _sumVisual.text = _sum.ToString();
    }

    public void RefreshMoney()
    {
        _sum = _savedMoney;
        _sumVisual.text = _sum.ToString();
    }

    public void AddMoney(int money)
    {
        _sum += money;
        _sumVisual.text = _sum.ToString();
    }

    public void TakeMoney(int money)
    {
        if (_sum >= money)
        {
            _sum -= money;
            _sumVisual.text = _sum.ToString();
        }
    }

    public void SaveMoney()
    {
        _savedMoney = _sum;
    }

    public bool HaveEnoughMoney(int money)
    {
        return _sum >= money ? true : false;
    }
}
