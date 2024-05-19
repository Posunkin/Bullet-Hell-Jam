using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class QuestCharacter : MonoBehaviour, IQuestable
{
    [SerializeField] private SpriteRenderer _dialogueMarker;

    [Header("Dialogue")]
    [SerializeField] private TextAsset _dialogue;
    [SerializeField] private int _goodChoice;
    [SerializeField] private int _badChoice;
    [SerializeField] private Loot _arifactPrefab;
    [SerializeField] private Room _room;
    [SerializeField] private bool _isDescisionable;
    private DialogueSystem _dialogueSystem;
    private StoryFlowHandler _storyFlowHandler;

    private bool _canStartDialogue;

    private InputAction _input;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, PlayerStats player, StoryFlowHandler storyFlowHandler)
    {
        _dialogueSystem = dialogueSystem;
        _input = player.GetComponent<PlayerController>().CurrentInput.Dialogue.Speak;
        _storyFlowHandler = storyFlowHandler;
    }

    private void Awake()
    {
        _canStartDialogue = true;
        _dialogueMarker.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_canStartDialogue)
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
        _canStartDialogue = false;
        _dialogueSystem.StartDialogue(_dialogue, this);
    }

    public void DialogueEnded(int index)
    {
        if (index == _goodChoice)
        {
            Loot key = Instantiate(_arifactPrefab, null);
            key.transform.position = new Vector2(transform.position.x + 3, transform.position.y - 3);
            if (_isDescisionable)
            {
                _storyFlowHandler.AddGoodDescision();
                _storyFlowHandler.LastDescisionWasGood = true;
            } 
        }
        else
        {
            _room.EnterQuestRoom();
            if (_isDescisionable) 
            {
                _storyFlowHandler.AddBadDescision();
                 _storyFlowHandler.LastDescisionWasGood = false;
            }
            Destroy(gameObject);
        }
        _storyFlowHandler.NextChapter();
    }
}
