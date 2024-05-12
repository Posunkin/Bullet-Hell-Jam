using UnityEngine;
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
    private DialogueSystem _dialogueSystem;

    private bool _canStartDialogue;

    private PlayerInput _input;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem)
    {
        _dialogueSystem = dialogueSystem;
    }

    private void Awake()
    {
        _canStartDialogue = true;
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
            if (_canStartDialogue)
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
        _canStartDialogue = false;
        _dialogueSystem.StartDialogue(_dialogue, this);
    }

    public void DialogueEnded(int index)
    {
        if (index == _goodChoice)
        {
            Loot key = Instantiate(_arifactPrefab, null);
            key.transform.position = new Vector2(transform.position.x + 3, transform.position.y - 3);
        }
        else
        {
            _room.EnterQuestRoom();
            Destroy(gameObject);
        }
    }
}
