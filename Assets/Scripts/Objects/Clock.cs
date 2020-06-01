using UnityEngine;
using System.Collections;
using System;

public class Clock : MonoBehaviour
{
    private Transform _smallArrow;
    private Transform _bigArrow;

    private void Awake()
    {
        _smallArrow = GetComponentsInChildren<Transform>()[1];
        _bigArrow = GetComponentsInChildren<Transform>()[2];
    }

    internal void SetTime(float currentHour)
    {
        Debug.Log("Time set to " + currentHour);
        _smallArrow.eulerAngles = new Vector3(0, 0, -30 * (currentHour % 12));
        _bigArrow.eulerAngles = new Vector3(0, 0, -360 * (currentHour % 1));
    }
}
