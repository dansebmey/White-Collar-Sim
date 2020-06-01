using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class WcsInteractArea : WcsUIObjectBase
{
    private Transform _interactButtonCanvas;
    protected WcsButton[] buttons;

    internal override void Awake()
    {
        base.Awake();

        _interactButtonCanvas = GetComponentsInChildren<Transform>(true)[1];
        if (_interactButtonCanvas != null)
        {
            WcsButton[] buttonsInChildren = _interactButtonCanvas.GetComponentsInChildren<WcsButton>(true);
            buttons = new WcsButton[buttonsInChildren.Length];
            if (buttonsInChildren != null && buttonsInChildren.Length > 0)
            {
                for (int i = 0; i < buttonsInChildren.Length; i++)
                {
                    buttons[i] = buttonsInChildren[i];
                }
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        ShowButtons();
        _interactButtonCanvas.gameObject.SetActive(true);

        foreach (InteractionButton button in buttons)
        {
            button.OnSetActive();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        _interactButtonCanvas.gameObject.SetActive(false);
    }

    private void ShowButtons()
    {
        foreach (InteractionButton b in buttons)
        {
            if (b.GetComponentInChildren<InteractionButton>().Text.Length > 0)
            {
                b.gameObject.SetActive(true);
            }
        }
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        Debug.Log("That tickles!");
    }
}
