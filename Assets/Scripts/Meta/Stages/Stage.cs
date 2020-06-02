using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] private Clock _clock;

    private void Start()
    {
        OnStageLoad();
    }

    protected virtual void OnStageLoad()
    {
        OnTimeChange(gameManager.CurrentHour);
    }

    public void OnTimeChange(float currentHour)
    {
        if (_clock != null)
            _clock.SetTime(currentHour);
    }
}