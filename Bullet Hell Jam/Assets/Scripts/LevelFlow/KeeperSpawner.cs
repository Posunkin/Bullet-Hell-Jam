using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class KeeperSpawner : MonoBehaviour, IQuestable
{
    [SerializeField] private KeeperBoss _weakKeeper;
    [SerializeField] private KeeperBoss _keeperGiant;
    [SerializeField] private KeeperBoss _keeperGoblin;
    [SerializeField] private KeeperBoss _keeperMax;
    [SerializeField] private PlayerStatsSO _descisions;
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private TextAsset[] _finalText;
    private DialogueSystem _dialogueSystem;
    private IInstantiator _instantiator;
    private int _indexText;

    [Inject]
    private void Construct(DialogueSystem dialogueSystem, IInstantiator instantiator)
    {
        _dialogueSystem = dialogueSystem;
        _instantiator = instantiator;
    }
    
    private void Awake()
    {
        SpawnKeeper();
    }

    private void Start()
    {
        MusicHandler.Instance.PlayBossMusic();
    }

    private void SpawnKeeper()
    {
        if (_descisions.ShadowDash && _descisions.HealthRecieve)
        {
            KeeperBoss go = _instantiator.InstantiatePrefabForComponent<KeeperBoss>(_keeperMax);
            go.OnKeeperDeath += KeeperDead;
            _indexText = 2;
            go.transform.position = _spawnPosition.transform.position;
        }
        else if (_descisions.ShadowDash && !_descisions.HealthRecieve)
        {
            KeeperBoss go = _instantiator.InstantiatePrefabForComponent<KeeperBoss>(_keeperGoblin);
            go.OnKeeperDeath += KeeperDead;
            _indexText = 1;
            go.transform.position = _spawnPosition.transform.position;
        }
        else if (!_descisions.ShadowDash && _descisions.HealthRecieve)
        {
            KeeperBoss go = _instantiator.InstantiatePrefabForComponent<KeeperBoss>(_keeperGiant);
            go.OnKeeperDeath += KeeperDead;
            _indexText = 1;
            go.transform.position = _spawnPosition.transform.position;
        }
        else
        {
            KeeperBoss go = _instantiator.InstantiatePrefabForComponent<KeeperBoss>(_weakKeeper);
            go.OnKeeperDeath += KeeperDead;
            _indexText = 0;
            go.transform.position = _spawnPosition.transform.position;
        }
    }

    private void KeeperDead(KeeperBoss keeper)
    {
        keeper.OnKeeperDeath -= KeeperDead;
        _dialogueSystem.StartDialogue(_finalText[_indexText], this);
    }

    public void DialogueEnded(int index)
    {
        SceneManager.LoadScene(5);
    }
}
