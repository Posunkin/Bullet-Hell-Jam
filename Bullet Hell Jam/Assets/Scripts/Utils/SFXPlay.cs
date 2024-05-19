using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _source.PlayOneShot(_clip);
    }
}
