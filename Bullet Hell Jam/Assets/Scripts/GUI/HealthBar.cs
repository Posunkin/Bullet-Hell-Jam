using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    public void SetHealth(float maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        _healthBar.value = _healthBar.maxValue;
    }

    public void ChangeHealth(float currentHealth)
    {
        _healthBar.value = currentHealth;
    }
}
