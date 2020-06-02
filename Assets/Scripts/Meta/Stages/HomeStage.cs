using UnityEngine;
using System.Collections;
using System;

public class HomeStage : Stage
{
    protected override void OnStageLoad()
    {
        HUDCanvas.GetInstance().gameObject.SetActive(false);
        StartCoroutine(TransitionToOffice(2));
    }

    private IEnumerator TransitionToOffice(int delay)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<SceneLoader>().ChangeScene("Office");
    }
}
