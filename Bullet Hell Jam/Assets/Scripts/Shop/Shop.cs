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
    private InputAction _input;
    private Wallet _wallet;
    private DialogueSystem _dialogueSystem;
    private int _healthPrice = 1;
    private int _pistolPrice = 1;
    private int _shotgunPrice = 1;
    private int _healthGradedCount = 0;
    private int _pistolDamageGradedCount = 0;
    private int _pistolDelayGradedCount = 0;
    private int _shotgunDamageGradedCount = 0;
    private int _shotgunDelayGradedCount = 0;

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
        _healthPriceUI.text = _healthPrice.ToString();
        _pistolPriceUI.text = _pistolPrice.ToString();
        _shotgunPriceUI.text = _shotgunPrice.ToString();
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
        if (_wallet.HaveEnoughMoney(_healthPrice) && _healthGradedCount < 5)
        {
            _wallet.TakeMoney(_healthPrice);
            _stats.GradeHealth();
            _healthPrice *= 2;
            _healthPriceUI.text = _healthPrice.ToString();
            _healthGradedCount++;
        }
    }

    public void GradePistolDamage()
    {
        if (_wallet.HaveEnoughMoney(_pistolPrice) && _pistolDamageGradedCount < 5)
        {
            _wallet.TakeMoney(_pistolPrice);
            _stats.GradePistolDamage();
            _pistolPrice *= 2;
            _pistolPriceUI.text = _pistolPrice.ToString();
            _pistolDamageGradedCount++;
        }
    }

    public void GradePistolDelay()
    {
        if (_wallet.HaveEnoughMoney(_pistolPrice) && _pistolDelayGradedCount < 5)
        {
            _wallet.TakeMoney(_pistolPrice);
            _stats.GradePistolDelay();
            _pistolPrice *= 2;
            _pistolPriceUI.text = _pistolPrice.ToString();
            _pistolDelayGradedCount++;
        }
    }

    public void GradeShotgunDamage()
    {
        if (_wallet.HaveEnoughMoney(_shotgunPrice) && _shotgunDamageGradedCount < 5)
        {
            _wallet.TakeMoney(_shotgunPrice);
            _stats.GradeShotgunDamage();
            _shotgunPrice *= 2;
            _shotgunPriceUI.text = _shotgunPrice.ToString();
            _shotgunDamageGradedCount++;
        }
    }

    public void GradeShotgunDelay()
    {
        if (_wallet.HaveEnoughMoney(_shotgunPrice) && _shotgunDelayGradedCount < 5)
        {
            _wallet.TakeMoney(_shotgunPrice);
            _stats.GradeShotgunDelay();
            _shotgunPrice *= 2;
            _shotgunPriceUI.text = _shotgunPrice.ToString();
            _shotgunDelayGradedCount++;
        }
    }


}
