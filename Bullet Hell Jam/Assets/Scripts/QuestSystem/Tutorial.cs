using UnityEngine;
using Zenject;

public class Tutorial : MonoBehaviour, IQuestable
{
    [SerializeField] private TextAsset _tutorial;

    private DialogueSystem _dialogueSystem;
    private StoryFlowHandler _storyFlowHandler;
    private bool _tutorialEnded;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, StoryFlowHandler storyFlowHandler)
    {
        _dialogueSystem = dialogueSystem;
        _storyFlowHandler = storyFlowHandler;
    }

    private void Start()
    {
        if (!_storyFlowHandler.TutorialEnded)
        {
            _tutorialEnded = false;
        }
        else
        {
            _tutorialEnded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(!_tutorialEnded) _dialogueSystem.StartDialogue(_tutorial, this);
    }

    public void DialogueEnded(int index)
    {
        _storyFlowHandler.EndTutorial();
        _tutorialEnded = true;
        Destroy(gameObject);
    }
}
