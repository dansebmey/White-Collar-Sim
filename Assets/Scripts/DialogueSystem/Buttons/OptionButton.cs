using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : WcsButton
{
    public int buttonIndex;

    protected override bool CanBeClicked()
    {
        return true;
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        dialogueManager.OptionSelected(buttonIndex);
    }
}
