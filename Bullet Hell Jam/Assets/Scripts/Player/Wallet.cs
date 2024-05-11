using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Sum { get => _sum; }

    [SerializeField] private TextMeshProUGUI _sumVisual;

    private int _sum;

    private void Start()
    {
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

    public bool HaveEnoughMoney(int money)
    {
        return _sum >= money ? true : false;
    }
}
