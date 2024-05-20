using System.Collections;
using UnityEngine;

public class SphereSummon : MonoBehaviour
{
    [SerializeField] private KeeperSphere _sphere;
    private KeeperBoss _boss;
    private float _recharge = 30f;
    private WaitForSeconds _rechargeTime;

    private void Awake()
    {
        _rechargeTime = new WaitForSeconds(_recharge);
        _sphere.OnSphereDeath += SphereDeath;
        _boss = GetComponent<KeeperBoss>();
    }

    private void Start()
    {
        _sphere.gameObject.SetActive(true);
        _sphere.Init();
        _boss.Immortality(true);
    }

    private void OnDisable()
    {
        _sphere.OnSphereDeath -= SphereDeath;
    }

    private void SphereDeath()
    {
        _boss.Immortality(false);
        StartCoroutine(RechargeRoutine());
    }

    private IEnumerator RechargeRoutine()
    {
        yield return _rechargeTime;
        _sphere.gameObject.SetActive(true);
        _sphere.transform.position = this.transform.position;
        _sphere.Init();
        _boss.Immortality(true);
    }
}
