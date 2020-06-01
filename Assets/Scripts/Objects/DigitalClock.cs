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

    internal override void SetTime(float hours)
    {
        int minutes = (int)((hours % 1) * 60);

        _textObject.text
            = Mathf.Floor(hours).ToString() 
            + ":" 
            + (minutes < 10 ? "0" : "")
            + ((hours % 1) * 6).ToString();
    }
}
