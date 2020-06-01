using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Stage _currentStage;

    private float _currentHour = 9.5f;
    public float CurrentHour
    {
        get => _currentHour;
        set
        {
            _currentHour = value;
            _currentStage.OnTimeChange(_currentHour);

            PlayerPrefs.SetFloat("CurrentHour", _currentHour);
        }
    }

    public DialogueManager _dialogueManager;

    public PlayerCharacter pc;

    private float _unavailableColleagueHours;
    public float UnavailableColleagueHours
    {
        get => _unavailableColleagueHours;
        set
        {
            _unavailableColleagueHours = value;
            PlayerPrefs.SetFloat("UnavailableColleagueHours", _unavailableColleagueHours);
        }
    }

    [HideInInspector] public enum State { FREE, DIALOGUE, CUTSCENE };
    private State _currentState = State.FREE;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
        }
    }

    private Image _dialogueFilter;
    private Animator _screenFadeFilterAnimator;

    private void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _currentStage = FindObjectOfType<Stage>();

        _screenFadeFilterAnimator = GetComponentsInChildren<Animator>(true)[0];

        Debug.Log("TEST: AWOKEN");
    }

    private void Start()
    {
        _dialogueManager.StartDialogue(DialogueManager.DialogueKey.DAY_1_BARRINGTON_GOOD_MORNING);

        CurrentHour = PlayerPrefs.GetFloat("CurrentHour");
        Debug.Log("Current hour = " + CurrentHour + " (according to PlayerPrefs: " + PlayerPrefs.GetFloat("CurrentHour"));
        UnavailableColleagueHours = PlayerPrefs.GetFloat("UnavailableColleagueHours");
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        _screenFadeFilterAnimator.Play("FadeToBlack");
        yield return new WaitForSeconds(0.5f);
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
    }

    public void MoveTimeForward(float hours)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        _currentState = State.CUTSCENE;
        _screenFadeFilterAnimator.Play("Fade_2s");

        CurrentHour += hours;

        UnavailableColleagueHours -= hours;
        if (CurrentHour >= 17)
        {
            EndWorkingDay();
        }
    }

    private void EndWorkingDay()
    {
        throw new NotImplementedException();
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
            pc.AlterWorkApproval(-2);
        }
        else if (_unavailableColleagueHours < 0)
        {
            _unavailableColleagueHours = 0;
        }
        _unavailableColleagueHours += 4;
    }
}
