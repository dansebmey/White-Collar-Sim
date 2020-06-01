using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class WcsButton : WcsUIObjectBase
{
    private Text _textObject;
    protected Button buttonObject;

    internal override void Awake()
    {
        base.Awake();

        _textObject = GetComponentInChildren<Text>();
        buttonObject = GetComponent<Button>();
    }

    internal virtual void OnSetActive()
    {

    }

    internal string Text { get => _textObject.text; set => _textObject.text = value; }
}
