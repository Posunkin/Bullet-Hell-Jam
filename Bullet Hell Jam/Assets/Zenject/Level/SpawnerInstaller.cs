using UnityEngine;
using Zenject;

public class SpawnerInstaller : MonoInstaller
{
    [SerializeField] private Spawner _spawner;

    public override void InstallBindings()
    {
        Container.Bind<Spawner>().FromInstance(_spawner).AsSingle();
    }
}