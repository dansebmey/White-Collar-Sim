using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    private static int _currentDay = 1;
    public int CurrentDay
    {
        get => _currentDay;
        set
        {
            _currentDay = value;

        }
    }

    private static float _currentHour = 9.5f;
    public float CurrentHour
    {
        get => _currentHour;
        set
        {
            _currentHour = value;
            FindObjectOfType<Stage>().OnTimeChange(_currentHour);
        }
    }

    internal PlayerCharacter pc;

    private float _unavailableColleagueHours;
    public float UnavailableColleagueHours
    {
        get => _unavailableColleagueHours;
        set
        {
            _unavailableColleagueHours = value;
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
            if(_currentState == State.FREE)
            {
                CheckForAppointments();
                if(FindObjectOfType<Stage>() is OfficeStage
                    && CurrentHour >= 13 && CurrentHour < 14
                    && pc.WorkloadInHours >= WorkingHoursLeftUntil(17) * 2)
                {
                    DialogueManager.GetInstance().StartConversation(DialogueManager.DialogueKey.WARNING_WORKING_TOO_LITTLE);
                }
            }
        }
    }

    private void CheckForAppointments()
    {
        foreach (Appointment appointment in pc.appointments)
        {
            if (appointment.IsToday(_currentDay)
                && !appointment.hasOccurredToday
                && (CurrentHour >= appointment.startingHour && CurrentHour < appointment.endingHour))
            {
                if(appointment.linkedDialogueKeyString != "")
                {
                    DialogueManager.GetInstance().LinkAppointmentToDialogue(appointment.linkedDialogueKeyString, appointment);
                }
                appointment.StartEvent();
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        pc = GetComponentInChildren<PlayerCharacter>();
    }

    public void NextDay()
    {
        FindObjectOfType<SceneLoader>().ChangeScene("Home");
        CurrentDay++;
        CurrentHour = 9.5f;
        
        foreach(Dialogue dialogue in DialogueManager.GetInstance().dialogueStorage.Values)
        {
            dialogue.WasHeld = false;
        }

        foreach(Appointment appointment in pc.appointments)
        {
            appointment.hasOccurredToday = false;
        }

        pc.StressLevel -= 4;
        pc.ConsecutiveHoursWorked = 0;
        pc.WorkloadInHours += 8;
    }

    public void MoveTimeForward(float hours, bool workRelated)
    {
        FindObjectOfType<SceneLoader>().Play("Fade_2s");

        if(workRelated)
        {
            pc.ConsecutiveHoursNotWorked = 0;
            pc.AlterConsecutiveHoursWorked(hours);
        }
        else
        {
            pc.AlterConsecutiveHoursNotWorked(hours);
        }

        CurrentHour += hours;
        UnavailableColleagueHours -= hours;
    }

    public void MoveTimeForward(float hours)
    {
        FindObjectOfType<SceneLoader>().Play("Fade_2s");

        pc.AlterConsecutiveHoursNotWorked(hours);

        CurrentHour += hours;
        UnavailableColleagueHours -= hours;
    }

    public void StartLunchBreak(float hours)
    {
        FindObjectOfType<SceneLoader>().Play("Fade_2s");

        pc.StressLevel -= 2;
        pc.WorkApproval++;
        pc.ResetConsecutiveHoursWorked();

        CurrentHour += hours;
        UnavailableColleagueHours -= hours;
    }

    internal bool ColleagueReadilyAvailable()
    {
        return _unavailableColleagueHours <= 0;
    }

    internal void OnColleagueAssistance()
    {
        DialogueManager.GetInstance().EndConversation();
        pc.AlterWorkload(-1);
        MoveTimeForward(0.5f, true);
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

    internal float WorkingHoursLeftUntil(int hour)
    {
        float extraSubtraction = 0;
        foreach(Appointment appointment in pc.appointments)
        {
            if(appointment.IsToday(CurrentDay)
                && appointment.isFormal
                && !appointment.hasOccurredToday
                && appointment.startingHour < hour)
            {
                extraSubtraction += appointment.Duration();
            }
        }
        return hour - CurrentHour - extraSubtraction;
    }

    public void ResetUnavailableColleagueHours()
    {
        UnavailableColleagueHours = 0;
    }
}
