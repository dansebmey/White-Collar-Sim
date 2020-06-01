using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractionButton : WcsButton
{
    private PlayerCharacter pc;

    public float durationInHours = 1;
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

    internal override void Awake()
    {
        base.Awake();

        pc = FindObjectOfType<PlayerCharacter>();
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        if(buttonObject.interactable) 
        {
            if (durationInHours > 0)
            {
                gameManager.MoveTimeForward(durationInHours);
            }
        }
    }

    internal override void OnSetActive()
    {
        buttonObject.interactable
            = pc.Workload >= minWorkload && pc.Workload <= maxWorkload
            && pc.WorkApproval >= minWorkApproval && pc.WorkApproval <= maxWorkApproval
            && pc.PartnerApproval >= minPartnerApproval && pc.PartnerApproval <= maxPartnerApproval
            && pc.FriendApproval >= minFriendApproval && pc.FriendApproval <= maxFriendApproval
            && pc.MomApproval >= minMomApproval && pc.MomApproval <= maxMomApproval
            && gameManager.IsTimeAvailable(durationInHours);
    }
}
