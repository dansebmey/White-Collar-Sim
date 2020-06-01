using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnswerableSentence : Sentence
{
    internal List<DialogueOption> AvailableOptions;
    private DialogueManager _conversationManager;
    
    public AnswerableSentence(int nodeIndex, DialogueManager conversationManager, string speakerName, string text, List<DialogueOption> availableChoices) : base(nodeIndex, conversationManager, speakerName, text)
    {
        _conversationManager = conversationManager;
        AvailableOptions = availableChoices;
    }

    internal DialogueOption OptionSelected(int optionIndex)
    {
        DialogueOption selectedOption = AvailableOptions[optionIndex];

        selectedOption.OnChoose();
        return selectedOption;
        // return new Sentence(selectedOption.nextNode, _conversationManager, "Harry", selectedOption.text);
    }
}
