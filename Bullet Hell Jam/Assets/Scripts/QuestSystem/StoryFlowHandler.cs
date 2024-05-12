using UnityEngine;

public class StoryFlowHandler : MonoBehaviour
{
    public int CurrentChapter { get => _currentChapter; }
    private int _currentChapter;

    private void Awake()
    {
        _currentChapter = 1;
    }

    public void NextChapter()
    {
        _currentChapter++;
    }
}
