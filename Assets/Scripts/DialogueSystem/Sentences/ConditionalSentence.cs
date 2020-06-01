using UnityEngine;
using System.Collections;
using System;

public class ConditionalSentence : Sentence
{
    private Func<bool> condition;
    internal int nextNodeIndexWhenConditionNotMet;

    public ConditionalSentence(int nodeIndex, DialogueManager dialogueManager, System.Func<bool> condition,
        int nextNodeIndexWhenConditionNotMet, string speakerName, string text, Action action = null)
        : base(nodeIndex, dialogueManager, speakerName, text, action)
    {
        this.condition = condition;
        this.nextNodeIndexWhenConditionNotMet = nextNodeIndexWhenConditionNotMet;
    }

    public ConditionalSentence(int nodeIndex, DialogueManager dialogueManager, System.Func<bool> condition,
        int nextNodeIndexWhenConditionNotMet, string speakerName, string text, int nextNodeIndex)
        : base(nodeIndex, dialogueManager, speakerName, text, nextNodeIndex)
    {
        this.condition = condition;
        this.nextNodeIndexWhenConditionNotMet = nextNodeIndexWhenConditionNotMet;
    }

    public bool IsConditionMet()
    {
        return condition.Invoke();
    }
}
