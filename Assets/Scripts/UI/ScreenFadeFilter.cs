using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFadeFilter : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetActiveFalseAndStateFree()
    {
        gameObject.SetActive(false);
        gameManager.CurrentState = GameManager.State.FREE;
    }
}