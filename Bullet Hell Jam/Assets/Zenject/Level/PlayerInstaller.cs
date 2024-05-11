using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerStats _playerPrefab;
    [SerializeField] private Transform _playerSpawnPosition;

    public override void InstallBindings()
    {
        BindPlayer();
    }

    private void BindPlayer()
    {
        PlayerStats player = Container.InstantiatePrefabForComponent<PlayerStats>(_playerPrefab, _playerSpawnPosition.position, Quaternion.identity, null);
        Container.Bind<PlayerStats>().FromInstance(player).AsSingle();
    }
}