using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DigitalClock : Clock
{
    private Text _textObject;

    private void Awake()
    {
        _textObject = GetComponent<Text>();
    }

    internal override void SetTime(float currentHour)
    {
        int minutes = (int)((currentHour % 1) * 60);

        _textObject.text
            = (currentHour < 10 ? "0" : "")
            + Mathf.Floor(currentHour).ToString()
            + ":"
            + (minutes < 10 ? "0" : "")
            + minutes;
    }
}
