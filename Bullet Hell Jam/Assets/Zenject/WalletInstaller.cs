using UnityEngine;
using Zenject;

public class WalletInstaller : MonoInstaller
{
    [SerializeField] private Wallet _walletPrefab;

    public override void InstallBindings()
    {
        Wallet wallet = Container.InstantiatePrefabForComponent<Wallet>(_walletPrefab);
        Container.Bind<Wallet>().FromInstance(wallet).AsSingle().NonLazy();
    }
}