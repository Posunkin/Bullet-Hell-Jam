using UnityEngine;

public class StoryFlowHandler : MonoBehaviour
{
    public int BadDescisions { get => _badDescisions; }
    public int GoodDescisions { get => _goodDescisions; }

    public bool TutorialEnded { get; private set; }
    public int CurrentChapter { get => _currentChapter; }
    private int _currentChapter;
    private int _badDescisions;
    private int _goodDescisions;

    private void Awake()
    {
        _currentChapter = 0;
    }

    public void NextChapter()
    {
        _currentChapter++;
    }

    public void EndTutorial()
    {
        TutorialEnded = true;
    }

    public void AddGoodDescision()
    {
        _goodDescisions++;
    }

    public void AddBadDescision()
    {
        _badDescisions++;
    }
}
