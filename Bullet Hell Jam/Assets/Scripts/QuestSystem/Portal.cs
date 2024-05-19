using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;

public class Portal : MonoBehaviour
{
    [SerializeField] private FadeTransition _fade;
    [SerializeField] private int _sceneToLoad;

    private float _waitTime = 1;
    private StoryFlowHandler _storyFlowHandler;
    private Wallet _wallet;
    private bool _entered;

    [Inject]
    private void Construct(Wallet wallet, StoryFlowHandler storyFlowHandler)
    {
        _wallet = wallet;
        _storyFlowHandler = storyFlowHandler;
    }

    private void Start()
    {
        _entered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _wallet.SaveMoney();
        if (other.GetComponent<PlayerStats>() != null && !_entered)
        {
            _entered = true;
            _storyFlowHandler.NextScene();
            Debug.Log(_storyFlowHandler.CurrentScene);
            _fade.gameObject.SetActive(true);
            _fade.FadeIn();
            StartCoroutine(LoadSceneRoutine());
        }        
    }

    public void InitPortal(int scene)
    {
        _sceneToLoad = scene;
    }    

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene(_sceneToLoad);
    }
}
