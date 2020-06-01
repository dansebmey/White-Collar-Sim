using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float _currentHour;
    public float CurrentHour
    {
        get => _currentHour;
        set
        {
            _currentHour = value;

            if (_clock != null)
                _clock.SetTime(_currentHour);
        }
    }

    private string _currentLocation;

    public DialogueManager _dialogueManager;

    public PlayerCharacter pc;
    private float _unavailableColleagueHours;

    [HideInInspector] public enum State { FREE, DIALOGUE, CUTSCENE };
    private State _currentState = State.FREE;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            _disableInteractionFilterImage.gameObject.SetActive(_currentState == State.DIALOGUE);
        }
    }

    private Image _disableInteractionFilterImage;
    private Animator _screenFadeFilterAnimator;

    private Clock _clock;

    private void Awake()
    {
        if (_dialogueManager == null) // TODO: Remove this
            _dialogueManager = FindObjectOfType<DialogueManager>();

        _screenFadeFilterAnimator = GetComponentsInChildren<Animator>(true)[0];
        _disableInteractionFilterImage = GetComponentsInChildren<Image>(true)[1];
        _clock = FindObjectOfType<Clock>();
    }

    private void Start()
    {
        CurrentHour = 9.5f;
        _dialogueManager.StartDialogue(DialogueManager.DialogueKey.DAY_1_BARRINGTON_GOOD_MORNING);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
        _currentLocation = sceneName;
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        _screenFadeFilterAnimator.Play("Fade_2s");
        yield return new WaitForSeconds(1);
        LoadScene(sceneName);
    }

    internal bool IsTimeAvailable(float hours)
    {
        // TODO: make this generic by looping through a collection of planned mandatory events
        // such as lunch and meetings, but not personally planned things such as private calls.
        // The character should have an agenda, basically. Or should they be allowed to forget things?
        if (CurrentHour < 12)
        {
            return CurrentHour + hours <= 12;
        }
        else if (CurrentHour < 17)
        {
            return CurrentHour + hours <= 17;
        }
        else return true;
    }

    public void StartConversation(string conversationSceneName, Dialogue conversation)
    {
        LoadScene(conversationSceneName);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Awake();
    }

    public void MoveTimeForward(float hours)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        _currentState = State.CUTSCENE;
        _screenFadeFilterAnimator.Play("Fade_2s");

        CurrentHour += hours;

        _unavailableColleagueHours -= hours;
        if (_currentHour >= 17)
        {
            EndWorkingDay();
        }
    }

    private void EndWorkingDay()
    {
        switch(_currentLocation)
        {
            case "Office":
                break;
            case "Home":
                break;
        }
    }

    internal bool ColleagueReadilyAvailable()
    {
        return _unavailableColleagueHours <= 0;
    }

    internal void OnColleagueAssistance()
    {
        _dialogueManager.EndConversation();
        pc.AlterWorkload(-1);
        MoveTimeForward(0.75f);
        if (_unavailableColleagueHours >= 4)
        {
            pc.AlterApproval_Work(-2);
        }
        else if (_unavailableColleagueHours < 0)
        {
            _unavailableColleagueHours = 0;
        }
        _unavailableColleagueHours += 4;
    }
}
