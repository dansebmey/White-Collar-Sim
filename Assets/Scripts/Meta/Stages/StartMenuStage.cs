using UnityEngine;
using System.Collections;

public class StartMenuStage : Stage
{
    protected override void OnStageLoad()
    {
        base.OnStageLoad();
        HUDCanvas.GetInstance().gameObject.SetActive(false);
    }
}
