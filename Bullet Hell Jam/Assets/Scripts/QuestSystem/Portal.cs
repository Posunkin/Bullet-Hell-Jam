using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private FadeTransition _fade;
    [SerializeField] private int _sceneToLoad;

    private float _waitTime = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            _fade.gameObject.SetActive(true);
            _fade.FadeIn();
            StartCoroutine(LoadSceneRoutine());
        }        
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene(_sceneToLoad);
    }
}
