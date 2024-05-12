using UnityEngine;
using Zenject;

public class DialogueSystemInstaller : MonoInstaller
{
    [SerializeField] private DialogueSystem _dialogueSystem;

    public override void InstallBindings()
    {
        DialogueSystem dialogueSystem = Container.InstantiatePrefabForComponent<DialogueSystem>(_dialogueSystem);
        Container.Bind<DialogueSystem>().FromInstance(dialogueSystem).AsSingle().NonLazy();
    }
}
