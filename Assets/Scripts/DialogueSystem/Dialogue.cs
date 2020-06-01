using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public bool exitNode;
    internal DialogueManager dialogueManager;
    public List<Sentence> sentences;

    public Dialogue(DialogueManager dialogueManager, List<Sentence> sentences)
    {
        this.dialogueManager = dialogueManager;
        this.sentences = sentences;
    }
}
