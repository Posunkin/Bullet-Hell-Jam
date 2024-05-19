using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Data Objects")]
public class PlayerStatsSO : ScriptableObject
{
    public float MaxHealth { get => _maxHealth; }
    public float DashCD { get => _dashCD; }
    public float PistolDamage { get => _pistolDamage; }
    public float PistolDelay { get => _pistolDelay; }
    public float ShotgunDamage { get => _shotgunDamage; }
    public float ShotgunDelay { get => _shotgunDelay; }
    public bool ShadowDash;
    public bool HealthRecieve;
    public int DashSceneRecieve = 2;
    public int HealthSceneRecieve = 4;
    public int ExtraHealth = 10;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _dashCD;
    [SerializeField] private float _pistolDamage;
    [SerializeField] private float _pistolDelay;
    [SerializeField] private float _shotgunDamage;
    [SerializeField] private float _shotgunDelay;

    public void GradeHealth()
    {
        _maxHealth += 2;
    }

    public void HealthReward()
    {
        HealthRecieve = true;
    }

    public void GradeDashCD()
    {
        _dashCD -= 0.1f;
    }

    public void GradePistolDamage()
    {
        _pistolDamage += 0.1f;
    }

    public void GradePistolDelay()
    {
        _pistolDelay -= 0.05f;
    }

    public void GradeShotgunDamage()
    {
        _shotgunDamage += 0.2f;
    }

    public void GradeShotgunDelay()
    {
        _shotgunDelay -= 0.05f;
    }
}
