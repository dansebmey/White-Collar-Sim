using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public string stageName;
    [SerializeField] private Clock _clock;

    public void OnTimeChange(float currentHour)
    {
        _clock.SetTime(currentHour);
    }
}