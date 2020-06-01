using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class DialogueManager : WcsUIObjectBase
{
    protected internal GameManager gameManager;

    [Header("JSON Input")]
    public TextAsset conversationsJson;
    public TextAsset conversantsJson;

    private Animator _animator;

    private Transform _playerSide;
    private Transform _otherSide;
    private OptionButton[] _optionButtons;

    private Text _playerSideName;
    private Text _otherSideName;
    private Text _playerSideText;

    private Text _otherSideText;

    private Image _playerSideChathead;
    private Image _otherSideChathead;

    public float typingSpeed = 0.01f;
    private bool _isCurrentlyTypingText;


    public Conversant player;

    private Dialogue _currentDialogue;
    private Sentence _currentSentence;
    private int _currentSentenceIndex;

    public enum DialogueKey 
    { 
        DAY_1_BARRINGTON_GOOD_MORNING, 
        ASKING_FOR_COLLEAGUE_ASSISTANCE
    }
    internal Dictionary<DialogueKey, Dialogue> dialogueStorage = new Dictionary<DialogueKey, Dialogue>();
    internal List<Conversant> conversants = new List<Conversant>();

    public enum ConversantKey
    {
        Harry, Barrington, Partner, Friend, Mom, Colleague
    }

    internal override void Awake()
    {
        base.Awake();
        gameManager = FindObjectOfType<GameManager>();

        _animator = GetComponent<Animator>();
        _playerSide = GetComponentsInChildrenWithTag<Transform>("DialogueSide")[0];
        _playerSideName = _playerSide.GetComponentsInChildren<Text>()[0];
        _playerSideText = _playerSide.GetComponentsInChildren<Text>()[1];
        _playerSideChathead = GetComponentsInChildrenWithTag<Image>("Chathead")[0];
        _otherSide = GetComponentsInChildrenWithTag<Transform>("DialogueSide")[1];
        _otherSideName = _otherSide.GetComponentsInChildren<Text>()[0];
        _otherSideText = _otherSide.GetComponentsInChildren<Text>()[1];
        _otherSideChathead = GetComponentsInChildrenWithTag<Image>("Chathead")[1];

        Transform optionRow = GetComponentsInChildrenWithTag<Transform>("OptionRow")[0];
        _optionButtons = new OptionButton[4];
        foreach(OptionButton button in optionRow.GetComponentsInChildren<OptionButton>())
        {
            _optionButtons[button.buttonIndex] = button;
        }

        gameObject.SetActive(false);

        conversants.Add(player = new Conversant("Harry", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/harry")));
        conversants.Add(new Conversant("Barrington", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/harry")));
        conversants.Add(new Conversant("Kenza", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/kenza")));

        GenerateDialogues();
    }

    private void GenerateDialogues()
    {
        dialogueStorage.Add(DialogueKey.DAY_1_BARRINGTON_GOOD_MORNING,
            new Dialogue(this,
            new List<Sentence>()
            {
                new Sentence(0, this, "Harry", "Good morning, Barry!"),
                new AnswerableSentence(1, this, "Barrington", "Hi there, Harrington. How are you today? " +
                "Feeling ready to tackle this project?",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, this, "Harry", "Sure am!", 2)),
                        new DialogueOption(new Sentence(-1, this, "Harry", "Eh, I suppose so.", 5),
                            new Action(() => gameManager.pc.AlterApproval_Work(1)))
                    }),
                new Sentence(2, this, "Barrington", "That's the spirit!"),
                new Sentence(3, this, "Barrington", "Have a nice day, Harry. And tell me if you need anything."),
                new Sentence(4, this, "Harry", "Will do, thanks!",
                    new Action(() => EndConversation())),
                new Sentence(5, this, "Barrington", "That doesn't sound too enthusiastic. Come on now, it won't be *that* bad!"),
                new Sentence(6, this, "Barrington", "Once this is over, you'll be more than able to " +
                "afford a proper vacation with your family!"),
                new Sentence(7, this, "Harry", "Fair enough. You'd better give me at *least* a month off, then, though!"),
                new Sentence(8, this, "Barrington", "Haha. I'm sure we can arrange something when the time comes."),
                new Sentence(9, this, "Barrington", "But for the coming weeks, I need you to give me your very best. " +
                "I have a feeling these guys may become customers for the long run, you know.",
                    new Action(() => EndConversation()))
            }));


        dialogueStorage.Add(DialogueKey.ASKING_FOR_COLLEAGUE_ASSISTANCE,
            new Dialogue(this,
            new List<Sentence>() // TODO: Check for other colleagues if one is unavailable
            {
                new Sentence(0, this, "Harry", "Hey, Kenza. Could you help me for a sec?"),
                new ConditionalSentence(1, this, new Func<bool>(() => gameManager.ColleagueReadilyAvailable()), 3,
                "Kenza", "Ummm... sure! Just one moment."),
                new Sentence(2, this, "Harry", "Awesome. Thank you!",
                    new Action(() => gameManager.OnColleagueAssistance())),
                new AnswerableSentence(3, this, "Kenza", "Sorry, I'm a bit too busy at the moment.",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, this, "Harry", "Pretty please? I really need a hand here.", 4)),
                        new DialogueOption(new Sentence(-1, this, "Harry", "Alright, no worries.",
                            new Action(() => EndConversation())))
                    }),
                new ConditionalSentence(4, this, new Func<bool>(() => gameManager.pc.CheckWorkApproval(-16, -6)), 6,
                "Kenza", "Not now, Harry. Sorry, but we all have a lot to do."),
                new AnswerableSentence(5, this, "Kenza", "You shouldn't always ask someone else to do your work for you.",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, this, "Harry", "Alrighty then... sorry for asking.",
                            new Action(() => EndConversation())),
                            new Action(() => gameManager.pc.AlterApproval_Work(-1))),
                        new DialogueOption(new Sentence(-1, this, "Harry", "Sorry. I won't disturb you anymore.",
                            new Action(() => EndConversation())))
                    }),
                new Sentence(6, this, "Kenza", "*sigh* Fine... but it better be a quick one.", 2)
            })) ;
    }

    private string RandomColleague()
    {
        int randomNo = UnityEngine.Random.Range(0, 3);
        switch(randomNo)
        {
            default:
                return "Kenza";
            case 0:
                return "Barbara";
            case 1:
                return "Garrett";
            case 2:
                return "Sylvio";
        }
    }

    protected override bool CanBeClicked()
    {
        return true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    internal Conversant GetConversant(string conversantName)
    {
        foreach(Conversant conversant in conversants)
        {
            if(conversant.name == conversantName)
            {
                return conversant;
            }
        }

        return null;
    }

    public void AskForColleagueAssistance_DUMMY() // TODO: Remove this
    {
        StartDialogue(DialogueKey.ASKING_FOR_COLLEAGUE_ASSISTANCE);
    }

    public void StartDialogue(string key)
    {
        if(Enum.TryParse(key, out DialogueKey dKey))
        {
            StartDialogue(dKey);
        }
        else
        {
            Debug.LogError("DialogueManager.StartDialogue(string key): DialogueKey [" + key + "] not found.");
        }
    }

    public void StartDialogue(DialogueKey key)
    {
        gameManager.CurrentState = GameManager.State.DIALOGUE;
        gameObject.SetActive(true);
        foreach(OptionButton button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        _currentDialogue = dialogueStorage[key];
        _currentSentenceIndex = 0;

        ShowSentence(_currentDialogue.sentences[_currentSentenceIndex]);
    }

    private void ShowSentence(Sentence sentence)
    {
        _currentSentence = sentence;
        bool isPlayerSpeaking = sentence.speaker == player;

        ChangeConversantName(sentence, isPlayerSpeaking);
        ChangeText(sentence, isPlayerSpeaking);
        if(!isPlayerSpeaking)
            _otherSideChathead.sprite = sentence.speaker.sprite;

        _playerSide.gameObject.SetActive(isPlayerSpeaking);
        _otherSide.gameObject.SetActive(!isPlayerSpeaking);
    }

    private void ChangeConversantName(Sentence dialogue, bool isPlayerSpeaking)
    {
        Text textObject = isPlayerSpeaking ? _playerSideName : _otherSideName;
        textObject.text = dialogue.speaker.name;
    }

    private void ChangeText(Sentence dialogue, bool isPlayerSpeaking)
    {
        Text textObject = isPlayerSpeaking ? _playerSideText : _otherSideText;
        StartCoroutine(TypeText(textObject, dialogue.text));
    }

    IEnumerator TypeText(Text textObject, string text)
    {
        _isCurrentlyTypingText = true;
        textObject.text = "";
        foreach (char c in text.ToCharArray())
        {
            if(!_isCurrentlyTypingText)
            {
                textObject.text = text;
                break;
            }
            float typeSpeed = typingSpeed;

            textObject.text += c;
            switch (c)
            {
                case '.':
                    typeSpeed = 0.4f;
                    break;
                case '?':
                    typeSpeed = 0.4f;
                    break;
                case '!':
                    typeSpeed = 0.4f;
                    break;
                case ',':
                    typeSpeed = 0.1f;
                    break;
            }

            yield return new WaitForSeconds(typeSpeed);
        }
        OnTypingFinished();
    }

    private void OnTypingFinished()
    {
        _isCurrentlyTypingText = false;
        if (_currentSentence is AnswerableSentence)
        {
            for (int i = 0; i < ((AnswerableSentence)_currentSentence).AvailableOptions.Count; i++)
            {
                _optionButtons[i].gameObject.SetActive(true);
            }
        }
    }

    internal void OptionSelected(int optionIndex)
    {
        foreach (OptionButton button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        DialogueOption selectedOption = ((AnswerableSentence)_currentSentence).OptionSelected(optionIndex);
        ShowSentence(selectedOption.sentence);
    }

    protected override void OnFullMouseClick(PointerEventData eventData)
    {
        if(_isCurrentlyTypingText)
        {
            _isCurrentlyTypingText = false;
        }
        else if (_currentSentence.action != null)
        {
            _currentSentence.action.Invoke();
        }
        else if (!(_currentSentence is AnswerableSentence))
        {
            NextSentence(_currentSentence.nextNodeIndex);
        }
    }
    
    private void NextSentence(int nodeIndex)
    {
        if (nodeIndex == -1)
        {
            _currentSentenceIndex++;
        } 
        else
        {
            _currentSentenceIndex = nodeIndex;
        }

        Sentence selectedSentence = _currentDialogue.sentences[_currentSentenceIndex];
        if (selectedSentence is ConditionalSentence && !((ConditionalSentence)selectedSentence).IsConditionMet())
        {
            selectedSentence = _currentDialogue.sentences[((ConditionalSentence)selectedSentence)
                .nextNodeIndexWhenConditionNotMet];
        }

        ShowSentence(selectedSentence);
    }

    internal void EndConversation()
    {
        _animator.Play("ConversationBar_FadeOut");
        gameManager.CurrentState = GameManager.State.FREE;
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
