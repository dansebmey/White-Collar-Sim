using UnityEngine;
using System.Collections;
using System;

public class DialogueOption
{
    private Action _onChooseAction;
    public Sentence sentence;

    public DialogueOption(Sentence sentence, Action onChoose = null)
    {
        this.sentence = sentence;
        _onChooseAction = onChoose;
    }

    internal virtual void OnChoose()
    {
        if (_onChooseAction != null)
            _onChooseAction.Invoke();
    }
}
