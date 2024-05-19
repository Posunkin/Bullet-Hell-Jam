using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public static MusicHandler Instance { get; private set; }
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _bossMusic;
    private bool _backgroundMusicPlaying;
    private AudioSource _source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _source = GetComponent<AudioSource>();
        _source.loop = true;
        PlayBackMusic();
    }

    public void PlayBackMusic()
    {
        if (_source.isPlaying) _source.Stop();
        _source.clip = _backgroundMusic;
        _source.Play();
    }

    public void PlayBossMusic()
    {
        if (_source.isPlaying) _source.Stop();
        _source.clip = _bossMusic;
        _source.Play();
    }
}
