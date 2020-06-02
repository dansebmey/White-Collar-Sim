using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    private static float _workload = 7;
    public float WorkloadInHours
    {
        get => _workload;
        set
        {
            _workload = Mathf.Clamp(value, 0, 80);
            HUDCanvas.GetInstance().UpdateWorkloadCounter(_workload);
        }
    }

    private static float _stressLevel = 0; // between 1 and 20
    // stress level should enable or disable certain (rational) arguments in conversation
    public float StressLevel
    {
        get => _stressLevel;
        set
        {
            _stressLevel = Mathf.Clamp(value, 0, 20);
            HUDCanvas.GetInstance().UpdateStressMeter(_stressLevel);
        }
    }

    private static float _workApproval = 0;
    public float WorkApproval
    {
        get => _workApproval;
        set
        {
            _workApproval = value;
        }
    }

    private static float _friendApproval = 0;
    public float FriendApproval
    {
        get => _friendApproval;
        set
        {
            _friendApproval = value;
        }
    }

    private static float _partnerApproval = 0;
    public float PartnerApproval
    {
        get => _partnerApproval;
        set
        {
            _partnerApproval = value;
        }
    }

    private static float _momApproval = 0;
    public float MomApproval
    {
        get => _momApproval;
        set
        {
            _momApproval = value;
        }
    }

    private static float _daysTakenOff = 0;
    public float DaysTakenOff
    {
        get => _daysTakenOff;
        set
        {
            _daysTakenOff = value;
        }
    }

    private static float _lunchesSkipped = 0;
    public float LunchesSkipped
    {
        get => _lunchesSkipped;
        set
        {
            _lunchesSkipped = value;
        }
    }

    private static float _consecutiveHoursWorked = 0;
    public float ConsecutiveHoursWorked
    {
        get => _consecutiveHoursWorked;
        set
        {
            _consecutiveHoursWorked = value;

            if (ConsecutiveHoursWorked >= 2.25f)
            {
                StressLevel += 1 + ((ConsecutiveHoursWorked - 2.25f) * 0.5f);
            }
            /*if (ConsecutiveHoursWorked >= 3.5f && StressLevel >= 4 && Workload <= GameManager.GetInstance().HoursRemainingUntil(17))
            {
                DialogueManager.GetInstance().StartConversation(DialogueManager.DialogueKey.WARNING_WORKING_TOO_MUCH);
            }*/
        }
    }

    private static float _consecutiveHoursNotWorked = 0;
    public float ConsecutiveHoursNotWorked
    {
        get => _consecutiveHoursNotWorked;
        set
        {
            _consecutiveHoursNotWorked = value;
            if (ConsecutiveHoursNotWorked >= ConsecutiveHoursWorked * 0.125f)
            {
                StressLevel--;
                ConsecutiveHoursWorked = 0;
            }
            else if (ConsecutiveHoursNotWorked >= 0.75f &&
                WorkloadInHours >= GameManager.GetInstance().WorkingHoursLeftUntil(17))
            {
                WorkApproval--;
                DialogueManager.GetInstance().StartConversation
                    (DialogueManager.DialogueKey.WARNING_WORKING_TOO_LITTLE);
            }
        }
    }

    public void ResetConsecutiveHoursWorked()
    {
        ConsecutiveHoursWorked = 0;
    }

    public List<Appointment> appointments;

    internal bool HasTimeAvailable(float currentHour, float activityDuration)
    {
        foreach(Appointment appointment in appointments)
        {
            if(!appointment.hasOccurredToday && appointment.isFormal && currentHour + activityDuration > appointment.startingHour)
            {
                return false;
            }
        }

        return true;
    }

    public void AlterWorkload(float amt)
    {
        if (amt < 0 && StressLevel >= 4)
        {
            float multiplier = 1 - ((StressLevel - 4) * 0.04f);
            amt *= multiplier;
        }

        WorkloadInHours += amt;
    }
    public void AlterStress(int amt) { StressLevel += amt; }
    public void AlterWorkApproval(int amt) { WorkApproval += amt; }
    public void AlterFriendApproval(int amt) { FriendApproval += amt; }
    public void AlterPartnerApproval(int amt) { PartnerApproval += amt; }
    public void AlterMomApproval(int amt) { MomApproval += amt; }
    public void AlterDaysTakenOff(int amt) { DaysTakenOff += amt; }
    public void AlterLunchesSkipped(int amt) { LunchesSkipped += amt; }
    public void AlterConsecutiveHoursWorked(float amt)
    { 
        ConsecutiveHoursWorked += amt;
    }
    public void AlterConsecutiveHoursNotWorked(float amt)
    {
        ConsecutiveHoursNotWorked += amt;
    }

    internal bool CheckWorkApproval(int min, int max)
    {
        return WorkApproval >= min && WorkApproval <= max;
    }
}
