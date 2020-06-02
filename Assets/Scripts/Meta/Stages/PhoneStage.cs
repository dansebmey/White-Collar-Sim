using UnityEngine;
using System.Collections;

public class PhoneStage : Stage
{
    protected override void OnStageLoad()
    {
        base.OnStageLoad();
        HUDCanvas.GetInstance().gameObject.SetActive(false);
    }
}
