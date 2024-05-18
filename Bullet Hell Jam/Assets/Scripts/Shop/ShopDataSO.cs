using UnityEngine;

[CreateAssetMenu(fileName = "Shop Data", menuName = "Shop data")]
public class ShopDataSO : ScriptableObject
{
    public int HealthPrice = 1;
    public int PistolPrice = 1;
    public int ShotgunPrice = 1;
    public int HealthGradeCount = 0;
    public int PistolGradeCount = 0;
    public int ShotgunGradeCount = 0;
}
