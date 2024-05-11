using Zenject;
using UnityEngine;

public class EnemyBulletPoolInstaller : MonoInstaller
{
    [SerializeField] private EnemyBulletPool _enemyBulletPool;
    [SerializeField] private PursuingBulletPool _pursuingBulletPool;

    public override void InstallBindings()
    {
        Container.Bind<EnemyBulletPool>().FromInstance(_enemyBulletPool).AsSingle();
        Container.Bind<PursuingBulletPool>().FromInstance(_pursuingBulletPool).AsSingle();
    }
}
