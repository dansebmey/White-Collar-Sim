using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionButton : WcsButton
{
    private PlayerCharacter pc;

    public float baseDurationInHours = 0;
    public bool workRelated;
    protected float durationInHours;

    [Range(0, 24)] public int notAvailableFromHour = 24;
    [Range(0, 24)] public int notAvailableUntilHour = 24;
    [Range(-16, 16)] public int minWorkload = -16;
    [Range(-16, 16)] public int maxWorkload = 16;
    [Range(-16, 16)] public int minWorkApproval = -16;
    [Range(-16, 16)] public int maxWorkApproval = 16;
    [Range(-16, 16)] public int minPartnerApproval = -16;
    [Range(-16, 16)] public int maxPartnerApproval = 16;
    [Range(-16, 16)] public int minFriendApproval = -16;
    [Range(-16, 16)] public int maxFriendApproval = 16;
    [Range(-16, 16)] public int minMomApproval = -16;
    [Range(-16, 16)] public int maxMomApproval = 16;

    protected Text textObject;

    internal override void Awake()
    {
        base.Awake();

        pc = FindObjectOfType<PlayerCharacter>();
        textObject = GetComponentInChildren<Text>();

        durationInHours = baseDurationInHours;
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        if(buttonObject.interactable) 
        {
            if (durationInHours > 0)
            {
                GameManager.GetInstance().MoveTimeForward(baseDurationInHours, workRelated);
            }
        }
    }

    internal override void OnSetActive()
    {
        buttonObject.interactable
            = !(GameManager.GetInstance().CurrentHour >= notAvailableFromHour 
            && GameManager.GetInstance().CurrentHour < notAvailableUntilHour)
            && pc.WorkloadInHours >= minWorkload && pc.WorkloadInHours <= maxWorkload
            && pc.WorkApproval >= minWorkApproval && pc.WorkApproval <= maxWorkApproval
            && pc.PartnerApproval >= minPartnerApproval && pc.PartnerApproval <= maxPartnerApproval
            && pc.FriendApproval >= minFriendApproval && pc.FriendApproval <= maxFriendApproval
            && pc.MomApproval >= minMomApproval && pc.MomApproval <= maxMomApproval
            && pc.HasTimeAvailable(GameManager.GetInstance().CurrentHour, durationInHours);
    }
}
