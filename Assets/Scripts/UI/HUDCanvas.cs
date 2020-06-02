using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDCanvas : WcsUIObjectBase
{
    static HUDCanvas instance;
    public static HUDCanvas GetInstance()
    {
        return instance;
    }

    private Transform _stressMeter;
    private Text _workloadCounter;
    private Text _workloadCounterShadow;

    internal override void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        base.Awake();

        // TODO: for some reason, these keep becoming unfindable after switching scenes

        _stressMeter = GetComponentsInChildren<Transform>(true)[2];
        _workloadCounter = GetComponentsInChildren<Text>(true)[4];
        _workloadCounterShadow = GetComponentsInChildren<Text>(true)[5];
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        Debug.Log("That tickles!");
    }

    public void UpdateStressMeter(float value)
    {
        _stressMeter.gameObject.SetActive(value > 0);
        _stressMeter.GetComponentInChildren<Slider>(true).value = value;
        // GetComponentsInChildren<Transform>(true)[2].gameObject.SetActive(value > 0);
        // GetComponentsInChildren<Transform>(true)[2].GetComponentInChildren<Slider>(true).value = value;
    }

    public void UpdateWorkloadCounter(float value)
    {
        _workloadCounter.text = value.ToString();
        _workloadCounterShadow.text = value.ToString();
        // GetComponentsInChildrenWithTag<Text>("WorkloadCounter")[0].text = value.ToString();
        // GetComponentsInChildrenWithTag<Text>("WorkloadCounter")[1].text = value.ToString();
    }
}
