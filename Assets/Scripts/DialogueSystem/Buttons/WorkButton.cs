using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class WorkButton : InteractionButton
{
    [Header("Duration modifiers")]
    [Range(0, 1)] public float perConsecutiveHourWorked;
    [Range(-16, 16)] public int forEveryCHWOver;
    [Space]
    [Range(0, 1)] public float perStressLevel;
    [Range(-16, 16)] public int forEveryStressLevelOver;

    internal override void OnSetActive()
    {
        PlayerCharacter pc = GameManager.GetInstance().pc;

        float increasedDurationRaw = baseDurationInHours;
        if(pc.ConsecutiveHoursWorked >= forEveryCHWOver)
        {
            increasedDurationRaw += (pc.ConsecutiveHoursWorked - forEveryCHWOver) * perConsecutiveHourWorked;
        }
        if(pc.StressLevel >= forEveryStressLevelOver)
        {
            increasedDurationRaw += (pc.StressLevel - forEveryStressLevelOver) * perStressLevel;
        }

        durationInHours = Mathf.FloorToInt(increasedDurationRaw / 0.25f) * 0.25f;
        textObject.text = "Get some work done (" + durationInHours + " hour" + (durationInHours == 1 ? ")" : "s)");

        base.OnSetActive();
    }
}
