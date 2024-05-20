using System;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public Action DialogueStarted;
    public Action DialogueEnded;

    [Header("Panel")]
    [SerializeField] private Canvas _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [Header("Choices"), Space(3)]
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject[] _choicesButtons;
    [SerializeField] private TextMeshProUGUI[] _choicesText;

    private Story _currentStory;
    private IQuestable _currentCharacter;
    private int _lastChoiceIndex;

    private void Start()
    {
        _lastChoiceIndex = -1;
        _dialoguePanel.gameObject.SetActive(false);
        foreach (var choice in _choicesButtons)
        {
            choice.SetActive(false);
        }
    }

    public void StartDialogue(TextAsset story, IQuestable character)
    {
        _currentStory = new Story(story.text);
        _currentCharacter = character;
        _dialoguePanel.gameObject.SetActive(true);
        DialogueStarted?.Invoke();

        ContinueStory();
    }

    public void StartDialogue(TextAsset story)
    {
        _currentStory = new Story(story.text);
        _dialoguePanel.gameObject.SetActive(true);
        DialogueStarted?.Invoke();

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogue();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        if (currentChoices.Count == 0)
        {
            return;
        }

        _continueButton.SetActive(false);

        for (int i = 0; i < currentChoices.Count; i++)
        {
            _choicesText[i].text = currentChoices[i].text;
            _choicesButtons[i].SetActive(true);
        }
    }

    public void MakeChoice(int choice)
    {
        _lastChoiceIndex = choice;
        _currentStory.ChooseChoiceIndex(choice);
        foreach (var but in _choicesButtons)
        {
            but.SetActive(false);
        }
        _continueButton.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogue()
    {
        DialogueEnded?.Invoke();
        _currentCharacter.DialogueEnded(_lastChoiceIndex);
        _currentCharacter = null;
        _dialoguePanel.gameObject.SetActive(false);
        _dialogueText.text = "";
        _lastChoiceIndex = -1;
    }

    public void BlockInput()
    {
        DialogueStarted?.Invoke();
    }

    public void UnblockInput()
    {
        DialogueEnded?.Invoke();
    }
}
