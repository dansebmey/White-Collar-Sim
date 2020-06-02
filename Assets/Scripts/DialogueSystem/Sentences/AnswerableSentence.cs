using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnswerableSentence : Sentence
{
    internal List<DialogueOption> AvailableOptions;
    
    public AnswerableSentence(int nodeIndex, string speakerName, string text, List<DialogueOption> availableChoices) : base(nodeIndex, speakerName, text)
    {
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
