using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    [HideInInspector] public float workload;

    [HideInInspector] public int stressLevel; // between 1 and 10
    // stress level should enable or disable certain (rational) arguments in conversation

    [HideInInspector] public int approval_Work;
    [HideInInspector] public int approval_Friend;
    [HideInInspector] public int approval_Partner;
    [HideInInspector] public int approval_Mom;

    [HideInInspector] public int daysTakenOff;

    public void AlterWorkload(float amt) { workload += amt; }
    public void AlterStress(int amt) { stressLevel += amt; }
    public void AlterApproval_Work(int amt) { approval_Work += amt; }
    public void AlterApproval_Friend(int amt) { approval_Friend += amt; }
    public void AlterApproval_Partner(int amt) { approval_Partner += amt; }
    public void AlterApproval_Mom(int amt) { approval_Mom += amt; }
    public void AlterDaysTakenOff(int amt) { daysTakenOff += amt; }

    internal bool CheckWorkApproval(int min, int max)
    {
        return approval_Work >= min && approval_Work <= max;
    }
}
