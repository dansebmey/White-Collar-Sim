using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class WcsUIObjectBase : MonoBehaviour, IPointerDownHandler, 
    IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected GameManager gameManager;
    protected DialogueManager dialogueManager;
    protected bool _isSelected;

    internal virtual void Awake() // TODO: InteractableUIObject should be split into 'WCSButton' and 'WCSClickableArea'
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        OnSelected();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if(_isSelected && CanBeClicked())
        {
            OnFullMouseClick(eventData);
        }
    }

    protected virtual bool CanBeClicked()
    {
        return gameManager.CurrentState == GameManager.State.FREE;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _isSelected = false;
        transform.localScale = new Vector3(1, 1, 1);
    }

    protected virtual void OnSelected()
    {

    }
    protected abstract void OnFullMouseClick(PointerEventData eventData);

    internal List<TComponent> GetComponentsInChildrenWithTag<TComponent>(string tagToFind)
    {
        var result = new List<TComponent>();
        var candidates = GetComponentsInChildren<TComponent>(true);

        foreach (var candidate in candidates)
        {
            if (candidate is Component component && component.gameObject.CompareTag(tagToFind))
            {
                result.Add(candidate);
            }
        }
        return result;
    }
}
