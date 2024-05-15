using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HubHost : MonoBehaviour, IQuestable
{
    [SerializeField] private SpriteRenderer _dialogueMarker;

    [Header("Dialogue")]
    [SerializeField] private TextAsset[] _dialoguesChapter1;
    [SerializeField] private Portal _portal;

    private bool _dialogueEnded;
    private int _currentChapter;
    private int _dialogueIndex;
    private DialogueSystem _dialogueSystem;
    private StoryFlowHandler _storyFlowHandler;
    private TextAsset[] _currentDialogue;

    private InputAction _input;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, StoryFlowHandler storyFlowHandler, PlayerStats player)
    {
        _dialogueSystem = dialogueSystem;
        _storyFlowHandler = storyFlowHandler;
        _input = player.GetComponent<PlayerController>().CurrentInput.Dialogue.Speak;
    }

    private void Awake()
    {
        _portal.gameObject.SetActive(false);
        _currentChapter = _storyFlowHandler.CurrentChapter;
        Debug.Log(_currentChapter);
        switch (_currentChapter)
        {
            case 1:
                _currentDialogue = _dialoguesChapter1;
                break;
        }
        _dialogueIndex = 0;
        _dialogueEnded = false;
        _dialogueMarker.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_dialogueEnded)
        {
            _dialogueMarker.enabled = true;
            if (_input.ReadValue<float>() > 0.5f)
            {
                Dialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _dialogueMarker.enabled = false;
    }

    private void Dialogue()
    {
        _dialogueSystem.StartDialogue(_currentDialogue[_dialogueIndex], this);
    }

    public void DialogueEnded(int index)
    {
        if (index == 0)
        {
            _dialogueEnded = true;
            _dialogueMarker.enabled = false;
            _portal.gameObject.SetActive(true);
        }
        else if (_dialogueIndex < _currentDialogue.Length - 1)
        {
            _dialogueIndex++;
        }
    }
}
