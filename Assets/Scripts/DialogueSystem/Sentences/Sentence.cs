using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Sentence
{
    public DialogueManager.ConversantKey conversantKey;

    public int nodeIndex;
    internal Conversant speaker;
    [TextArea(3, 10)] public string text;
    public List<DialogueOption> dialogueOptions;
    public int nextNodeIndex = -1;

    internal Action action;

    public Sentence(int nodeIndex, string speakerName, string text, Action action = null)
    {
        this.nodeIndex = nodeIndex;

        this.speaker = DialogueManager.GetInstance().GetConversant(speakerName);
        this.text = text;

        this.action = action;
    }

    public Sentence(int nodeIndex, string speakerName, string text, int nextNodeIndex)
    {
        this.nodeIndex = nodeIndex;

        this.speaker = DialogueManager.GetInstance().GetConversant(speakerName);
        this.text = text;

        this.nextNodeIndex = nextNodeIndex;
    }
}
