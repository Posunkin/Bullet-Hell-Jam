using UnityEngine;
using Zenject;

public class StoryFlowInstaller : MonoInstaller
{
    [SerializeField] private StoryFlowHandler _storyFlow;

    public override void InstallBindings()
    {
        StoryFlowHandler flow = Container.InstantiatePrefabForComponent<StoryFlowHandler>(_storyFlow);
        Container.Bind<StoryFlowHandler>().FromInstance(flow).AsSingle().NonLazy();
    }
}