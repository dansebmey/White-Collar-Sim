using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    private float _workload;
    public float Workload
    {
        get => _workload;
        set
        {
            _workload = value;
            PlayerPrefs.SetFloat("Player_Workload", _workload);
        }
    }

    private float _stressLevel; // between 1 and 10
    // stress level should enable or disable certain (rational) arguments in conversation
    public float StressLevel
    {
        get => _stressLevel;
        set
        {
            _stressLevel = value;
            PlayerPrefs.SetFloat("Player_StressLevel", _stressLevel);
        }
    }

    private float _workApproval;
    public float WorkApproval
    {
        get => _workApproval;
        set
        {
            _workApproval = value;
            PlayerPrefs.SetFloat("Player_WorkApproval", _workApproval);
        }
    }

    private float _friendApproval;
    public float FriendApproval
    {
        get => _friendApproval;
        set
        {
            _friendApproval = value;
            PlayerPrefs.SetFloat("Player_FriendApproval", _friendApproval);
        }
    }

    private float _partnerApproval;
    public float PartnerApproval
    {
        get => _partnerApproval;
        set
        {
            _partnerApproval = value;
            PlayerPrefs.SetFloat("Player_PartnerApproval", _partnerApproval);
        }
    }

    private float _momApproval;
    public float MomApproval
    {
        get => _momApproval;
        set
        {
            _momApproval = value;
            PlayerPrefs.SetFloat("Player_MomApproval", _momApproval);
        }
    }

    private float _daysTakenOff;
    public float DaysTakenOff
    {
        get => _daysTakenOff;
        set
        {
            _daysTakenOff = value;
            PlayerPrefs.SetFloat("Player_DaysTakenOff", _daysTakenOff);
        }
    }

    public void AlterWorkload(float amt) { Workload += amt; }
    public void AlterStress(int amt) { StressLevel += amt; }
    public void AlterWorkApproval(int amt) { WorkApproval += amt; }
    public void AlterFriendApproval(int amt) { FriendApproval += amt; }
    public void AlterPartnerApproval(int amt) { PartnerApproval += amt; }
    public void AlterMomApproval(int amt) { MomApproval += amt; }
    public void AlterDaysTakenOff(int amt) { DaysTakenOff += amt; }

    internal bool CheckWorkApproval(int min, int max)
    {
        return WorkApproval >= min && WorkApproval <= max;
    }
}
