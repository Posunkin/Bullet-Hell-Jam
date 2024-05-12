using UnityEngine;
using Zenject;

public class DialogueSystemInstaller : MonoInstaller
{
    [SerializeField] private DialogueSystem _dialogueSystem;

    public override void InstallBindings()
    {
        Container.Bind<DialogueSystem>().FromInstance(_dialogueSystem).AsSingle().NonLazy();
    }
}
