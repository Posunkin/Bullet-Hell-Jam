using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Shop : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _marker;

    [Header("Shop")]
    [SerializeField] private Canvas _shopMenu;
    [SerializeField] private PlayerStatsSO _stats;
    [SerializeField] private TextMeshProUGUI _healthPriceUI;
    [SerializeField] private TextMeshProUGUI _pistolPriceUI;
    [SerializeField] private TextMeshProUGUI _shotgunPriceUI;
    [SerializeField] private ShopDataSO _shopData;

    [SerializeField] private GameObject _healthActiveWindow;
    [SerializeField] private GameObject _pistolActiveWindow;
    [SerializeField] private GameObject _shotgunActiveWindow;
    [SerializeField] private GameObject _healthDeactiveWindow;
    [SerializeField] private GameObject _pistolDeactiveWindow;
    [SerializeField] private GameObject _shotgunDeactiveWindow;
    private InputAction _input;
    private Wallet _wallet;
    private DialogueSystem _dialogueSystem;

    [Inject]
    private void Construct(Wallet wallet, PlayerStats player, DialogueSystem dialogueSystem)
    {
        _wallet = wallet;
        _input = player.GetComponent<PlayerController>().CurrentInput.Dialogue.Speak;
        _dialogueSystem = dialogueSystem;
    }

    private void Awake()
    {
        _marker.enabled = false;
        _shopMenu.gameObject.SetActive(false);
        
        _healthPriceUI.text = _shopData.HealthPrice.ToString();
        _pistolPriceUI.text = _shopData.PistolPrice.ToString();
        _shotgunPriceUI.text = _shopData.ShotgunPrice.ToString();
        ActiveWindows();
    }

    private void ActiveWindows()
    {
        if (_shopData.HealthGradeCount == 4)
        {
            _healthActiveWindow.SetActive(false);
            _healthDeactiveWindow.SetActive(true);
        }
        if (_shopData.PistolGradeCount == 6)
        {
            _pistolActiveWindow.SetActive(false);
            _pistolDeactiveWindow.SetActive(true);
        }
        if (_shopData.ShotgunGradeCount == 6)
        {
            _shotgunActiveWindow.SetActive(false);
            _shotgunDeactiveWindow.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _marker.enabled = true;
        if (_input.ReadValue<float>() > 0.5f)
        {
            _dialogueSystem.BlockInput();
            _shopMenu.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _marker.enabled = false;
    }

    public void ExitFromShop()
    {
        _dialogueSystem.UnblockInput();
        _shopMenu.gameObject.SetActive(false);
    }

    public void GradeHealth()
    {
        if (_wallet.HaveEnoughMoney(_shopData.HealthPrice) && _shopData.HealthGradeCount < 5)
        {
            _wallet.TakeMoney(_shopData.HealthPrice);
            _stats.GradeHealth();
            int price = _shopData.HealthPrice * 2;
            _shopData.HealthPrice = price;
            _healthPriceUI.text = _shopData.HealthPrice.ToString();
            _shopData.HealthGradeCount = _shopData.HealthGradeCount + 1;
        }
        if (_shopData.HealthGradeCount == 4)
        {
            _healthActiveWindow.SetActive(false);
            _healthDeactiveWindow.SetActive(true);
        }
    }

    public void GradePistolDamage()
    {
        if (_wallet.HaveEnoughMoney(_shopData.PistolPrice) && _shopData.PistolGradeCount < 7)
        {
            _wallet.TakeMoney(_shopData.PistolPrice);
            _stats.GradePistolDamage();
            _shopData.PistolPrice *= 2;
            _pistolPriceUI.text = _shopData.PistolPrice.ToString();
            _shopData.PistolGradeCount++;
        }
        if (_shopData.PistolGradeCount == 6)
        {
            _pistolActiveWindow.SetActive(false);
            _pistolDeactiveWindow.SetActive(true);
        }
    }

    public void GradePistolDelay()
    {
        if (_wallet.HaveEnoughMoney(_shopData.PistolPrice) && _shopData.PistolGradeCount < 7)
        {
            _wallet.TakeMoney(_shopData.PistolPrice);
            _stats.GradePistolDelay();
            _shopData.PistolPrice *= 2;
            _pistolPriceUI.text = _shopData.PistolPrice.ToString();
            _shopData.PistolGradeCount++;
        }
        if (_shopData.PistolGradeCount == 6)
        {
            _pistolActiveWindow.SetActive(false);
            _pistolDeactiveWindow.SetActive(true);
        }
    }

    public void GradeShotgunDamage()
    {
        if (_wallet.HaveEnoughMoney(_shopData.ShotgunPrice) && _shopData.ShotgunGradeCount < 7)
        {
            _wallet.TakeMoney(_shopData.ShotgunPrice);
            _stats.GradeShotgunDamage();
            _shopData.ShotgunPrice *= 2;
            _shotgunPriceUI.text = _shopData.ShotgunPrice.ToString();
            _shopData.ShotgunGradeCount++;
        }
        if (_shopData.ShotgunGradeCount == 6)
        {
            _shotgunActiveWindow.SetActive(false);
            _shotgunDeactiveWindow.SetActive(true);
        }
    }

    public void GradeShotgunDelay()
    {
        if (_wallet.HaveEnoughMoney(_shopData.ShotgunPrice) && _shopData.ShotgunGradeCount < 7)
        {
            _wallet.TakeMoney(_shopData.ShotgunPrice);
            _stats.GradeShotgunDelay();
            _shopData.ShotgunPrice *= 2;
            _shotgunPriceUI.text = _shopData.ShotgunPrice.ToString();
            _shopData.ShotgunGradeCount++;
        }
        if (_shopData.ShotgunGradeCount == 6)
        {
            _shotgunActiveWindow.SetActive(false);
            _shotgunDeactiveWindow.SetActive(true);
        }
    }
}
