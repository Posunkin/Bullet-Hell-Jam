using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private float _messageTime;
    private WaitForSeconds _waitTime;

    private const string ARTIFACT_MESSAGE = "Portal is open!";
    private const string DASH_MESSAGE = "Shadow Dash available!";
    private const string HEALTH_MESSAGE = "Health UP!";

    private void Start()
    {
        _waitTime = new WaitForSeconds(_messageTime);
        _message.gameObject.SetActive(false);
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
}
