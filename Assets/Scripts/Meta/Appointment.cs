using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[System.Serializable]
public class Appointment
{
    public string appointmentName;

    public bool isFormal = true;

    [Range(1, 14)] public int day = 1;
    public bool repeatEveryDay = false;

    [Range(0, 24)] public float startingHour;
    [Range(0, 24)] public float endingHour;

    public string linkedDialogueKeyString;

    public UnityEvent triggerEvent;
    public UnityEvent attendEvent;
    public UnityEvent cancelEvent;

    internal bool hasOccurredToday = false;

    internal void StartEvent()
    {
        triggerEvent.Invoke();
        hasOccurredToday = true;
    }

    internal void Attend()
    {
        attendEvent.Invoke();
    }

    internal void Cancel()
    {
        cancelEvent.Invoke();
    }

    internal bool IsToday(int currentDay)
    {
        return day == currentDay || (currentDay >= day && repeatEveryDay);
    }

    internal float Duration()
    {
        return endingHour - startingHour;
    }
}
