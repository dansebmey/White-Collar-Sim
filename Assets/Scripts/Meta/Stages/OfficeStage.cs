using UnityEngine;
using System.Collections;

public class OfficeStage : Stage
{
    protected override void OnStageLoad()
    {
        base.OnStageLoad();
        HUDCanvas.GetInstance().gameObject.SetActive(true);

        if(GameManager.GetInstance().CurrentDay == 1)
        {
            DialogueManager.GetInstance().StartConversation(DialogueManager.DialogueKey.DAY_1_BARRINGTON_GOOD_MORNING);
        }
    }
}
