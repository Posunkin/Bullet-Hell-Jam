using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private float _messageTime;
    [SerializeField] private GameObject _pauseMenu;
    private bool _isPaused = false;
    private bool _pauseButtonClick = false;
    private WaitForSeconds _waitTime;
    private WaitForSeconds _pauseButtonDelay;

    private const string ARTIFACT_MESSAGE = "Portal is open!";
    private const string DASH_MESSAGE = "Shadow Dash available!";
    private const string HEALTH_MESSAGE = "Health UP!";

    private void Start()
    {
        _waitTime = new WaitForSeconds(_messageTime);
        _pauseButtonDelay = new WaitForSeconds(0.5f);
        _pauseButtonClick = false;
        _message.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !_pauseButtonClick)
        {
            _pauseButtonClick = true;
            StartCoroutine(PauseButtonRoutine());
            PauseGame();
        }
    }

    public void ArtifactMessage()
    {
        _message.text = ARTIFACT_MESSAGE;
        _message.gameObject.SetActive(true);
        StartCoroutine(MessageRoutine());
    }

    public void DashMessage()
    {
        _message.text = DASH_MESSAGE;
        _message.gameObject.SetActive(true);
        StartCoroutine(MessageRoutine());
    }

    public void HealthMessage()
    {
        _message.text = HEALTH_MESSAGE;
        _message.gameObject.SetActive(true);
        StartCoroutine(MessageRoutine());
    }

    private IEnumerator MessageRoutine()
    {
        yield return _waitTime;
        _message.gameObject.SetActive(false);
        _message.text = "";
    }

    private IEnumerator PauseButtonRoutine()
    {
        yield return _pauseButtonDelay;
        _pauseButtonClick = false;
    }

    public void PauseGame()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            _isPaused = true;
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
