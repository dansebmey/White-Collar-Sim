using UnityEngine;
using System.Collections;
using System;

public class AnalogueClock : Clock
{
    private Transform _smallArrow;
    private Transform _bigArrow;

    private void Awake()
    {
        _smallArrow = GetComponentsInChildren<Transform>()[1];
        _bigArrow = GetComponentsInChildren<Transform>()[2];
    }

    internal override void SetTime(float currentHour)
    {
        _smallArrow.eulerAngles = new Vector3(0, 0, -30 * (currentHour % 12));
        _bigArrow.eulerAngles = new Vector3(0, 0, -360 * (currentHour % 1));
    }
}
