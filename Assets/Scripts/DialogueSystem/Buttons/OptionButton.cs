using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : WcsButton
{
    public bool isEnabled = true;
    public int buttonIndex;

    protected override bool CanBeClicked()
    {
        return true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        DialogueManager.GetInstance().PreviewDialogueOption(buttonIndex);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        DialogueManager.GetInstance().HideDialogueOption();
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        if(isEnabled)
        {
            AudioManager.GetInstance().Play("click", 0.05f);
            DialogueManager.GetInstance().OptionSelected(buttonIndex);
        }
    }
}
