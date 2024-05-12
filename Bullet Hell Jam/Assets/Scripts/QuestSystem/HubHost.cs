using UnityEngine;
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

    private PlayerInput _input;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, StoryFlowHandler storyFlowHandler)
    {
        _dialogueSystem = dialogueSystem;
        _storyFlowHandler = storyFlowHandler;
    }

    private void Awake()
    {
        _portal.gameObject.SetActive(false);
        _currentChapter = _storyFlowHandler.CurrentChapter;
        switch (_currentChapter)
        {
            case 1:
                _currentDialogue = _dialoguesChapter1;
                break;
        }
        _dialogueIndex = 0;
        _dialogueEnded = false;
        _dialogueMarker.enabled = false;
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            if (!_dialogueEnded)
            {
                _dialogueMarker.enabled = true;
                _input.Enable();
                _input.Dialogue.Speak.performed += _ => Dialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            _dialogueMarker.enabled = false;
            _input.Disable();
        }
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
            _input.Disable();
            _portal.gameObject.SetActive(true);
        }
        else if (_dialogueIndex < _currentDialogue.Length - 1)
        {
            _dialogueIndex++;
        }
    }
}
