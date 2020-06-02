using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class DialogueManager : WcsUIObjectBase
{
    static DialogueManager instance;
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    [Header("JSON Input")]
    public TextAsset conversationsJson;
    public TextAsset conversantsJson;

    private Animator _animator;
    private Image _dialogueFilter;

    private CanvasGroup _playerSide;
    private CanvasGroup _otherSide;

    private OptionButton[] _playerSideOptionButtons;
    private OptionButton[] _otherSideOptionButtons;

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
        ASKING_FOR_COLLEAGUE_ASSISTANCE,
        COMING_FOR_LUNCH_QUESTION,
        WARNING_WORKING_TOO_MUCH,
        WARNING_WORKING_TOO_LITTLE,
        RANDOM_DAY_GOOD_MORNING,
        HARRY_IS_ASKED_FOR_HELP,
        WARNING_TOO_MUCH_WORKLOAD
    }
    internal Dictionary<DialogueKey, Dialogue> dialogueStorage = new Dictionary<DialogueKey, Dialogue>();
    internal List<Conversant> conversants = new List<Conversant>();

    public enum ConversantKey
    {
        Harry, Barrington, Partner, Friend, Mom, Colleague
    }

    internal override void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        base.Awake();

        _dialogueFilter = GetComponentsInChildren<Image>(true)[0];

        _animator = GetComponent<Animator>();
        _playerSide = GetComponentsInChildrenWithTag<CanvasGroup>("DialogueSide")[0];
        _playerSideName = _playerSide.GetComponentsInChildren<Text>()[0];
        _playerSideText = _playerSide.GetComponentsInChildren<Text>()[1];
        _playerSideChathead = GetComponentsInChildrenWithTag<Image>("Chathead")[0];
        _otherSide = GetComponentsInChildrenWithTag<CanvasGroup>("DialogueSide")[1];
        _otherSideName = _otherSide.GetComponentsInChildren<Text>()[0];
        _otherSideText = _otherSide.GetComponentsInChildren<Text>()[1];
        _otherSideChathead = GetComponentsInChildrenWithTag<Image>("Chathead")[1];

        Transform optionRow = GetComponentsInChildrenWithTag<Transform>("OptionRow")[0];
        _playerSideOptionButtons = new OptionButton[4];
        foreach (OptionButton button in optionRow.GetComponentsInChildren<OptionButton>())
        {
            _playerSideOptionButtons[button.buttonIndex] = button;
        }
        optionRow = GetComponentsInChildrenWithTag<Transform>("OptionRow")[1];
        _otherSideOptionButtons = new OptionButton[4];
        foreach (OptionButton button in optionRow.GetComponentsInChildren<OptionButton>())
        {
            _otherSideOptionButtons[button.buttonIndex] = button;
        }

        gameObject.SetActive(false);

        conversants.Add(player = new Conversant("Harry", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/harry")));
        conversants.Add(new Conversant("Barrington", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/harry")));
        conversants.Add(new Conversant("Kenza", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/kenza")));
        conversants.Add(new Conversant("Carlos", Resources.Load<Sprite>("Sprites/Conversation/Chatheads/carlos")));

        GenerateDialogues();
    }

    private void GenerateDialogues()
    {
        dialogueStorage.Add(DialogueKey.DAY_1_BARRINGTON_GOOD_MORNING,
            new Dialogue(true,
            new List<Sentence>()
            {
                new Sentence(0, "Barrington", "Hi there, Harrington."),
                new Sentence(1, "Harry", "Good morning, Barry."),
                new AnswerableSentence(2, "Barrington", "How are you today? " +
                "Feeling ready to tackle this project?",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Sure do!", 3)),
                        new DialogueOption(new Sentence(-1, "Harry", "Eh, I suppose so.", 6),
                            new Action(() => GameManager.GetInstance().pc.AlterWorkApproval(1)))
                    }),
                new Sentence(3, "Barrington", "That's the spirit!"),
                new Sentence(4, "Barrington", "Have a nice day, Harry. And tell me if you need anything."),
                new Sentence(5, "Harry", "Will do. Thanks!",
                    new Action(() => EndConversation())),
                new Sentence(6, "Barrington", "That doesn't sound too enthusiastic. Come on now, it won't be *that* bad!"),
                new Sentence(7, "Barrington", "Once this is over, you'll be more than able to " +
                "afford a proper vacation with your family!"),
                new Sentence(8, "Harry", "Fair enough. You'd better give me at *least* a month off then, though!"),
                new Sentence(9, "Barrington", "Haha. No promises, but I'm sure we can arrange something."),
                new Sentence(10, "Barrington", "But for the coming weeks, I need you to give me your very best. " +
                "I have a feeling these guys may become customers for the long run, you know.",
                    new Action(() => EndConversation()))
            }));

        dialogueStorage.Add(DialogueKey.RANDOM_DAY_GOOD_MORNING,
            new Dialogue(true,
            new List<Sentence>()
            {
                new Sentence(0, "Barrington", "Good morning, Harry!"),
                new Sentence(1, "Harry", "Good morning, Barry."),
                new AnswerableSentence(2, "Barrington", "How's things today?",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Pretty good, thanks!", 3)),
                        new DialogueOption(new Sentence(-1, "Harry", "Pretty good, thanks! How about yourself?", 4)),
                        new DialogueOption(new Sentence(-1, "Harry", "Eh, I've been better.", 6))
                    }),
                new Sentence(3, "Barrington", "Good. I won't disturb you any further. There's plenty of work left to do.",
                    new Action(() => EndConversation())),
                new Sentence(4, "Barrington", "Good! I'm doing quite well actually, thanks."),
                new Sentence(5, "Barrington", "Oh, you won't believe what happened to " +
                    "Erica at the supermarket yesterday...",
                    new Action(() => AttendCurrentAppointment())),
                /*new Sentence(6, "Barrington", "Hahaha! Ohhh, you should've been there Harrington. You would've laughed " +
                "your socks off!"),
                new Sentence(7, "Harry", "I bet!", 3),*/
                new Sentence(6, "Barrington", "Worrying response, yet not out of character."),
                new Sentence(7, "Barrington", "I hope you'll let me know if there's anything I can do to " +
                "make your working days more comfortable."),
                new Sentence(8, "Harry", "Thanks, Barry.", 3)
            }));

        dialogueStorage.Add(DialogueKey.ASKING_FOR_COLLEAGUE_ASSISTANCE,
            new Dialogue(false,
            new List<Sentence>() // TODO: Check for other colleagues if one is unavailable
            {
                new Sentence(0, "Harry", "Hey, Kenza. Could you help me for a sec?"),
                new ConditionalSentence(1, new Func<bool>(() => GameManager.GetInstance().ColleagueReadilyAvailable()), 3,
                "Kenza", "Ummm... sure! Just one moment."),
                new Sentence(2, "Harry", "Awesome. Thank you!",
                    new Action(() => GameManager.GetInstance().OnColleagueAssistance())),
                new AnswerableSentence(3, "Kenza", "Sorry, I'm a bit too busy at the moment.",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Pretty please? I really need a hand here.", 4)),
                        new DialogueOption(new Sentence(-1, "Harry", "Alright, no worries.",
                            new Action(() => EndConversation())))
                    }),
                new ConditionalSentence(4, new Func<bool>(() => GameManager.GetInstance().pc.CheckWorkApproval(-16, -6)), 6,
                "Kenza", "Not now, Harry. Sorry, but we all have a lot to do."),
                new AnswerableSentence(5, "Kenza", "You shouldn't always ask someone else to do your work for you.",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Alrighty then... sorry for asking.",
                            new Action(() => EndConversation())),
                            new Action(() => GameManager.GetInstance().pc.AlterWorkApproval(-1))),
                        new DialogueOption(new Sentence(-1, "Harry", "Sorry. I won't disturb you anymore.",
                            new Action(() => EndConversation())))
                    }),
                new Sentence(6, "Kenza", "*sigh* Fine... but it better be a quick one.", 2)
            }));

        dialogueStorage.Add(DialogueKey.COMING_FOR_LUNCH_QUESTION,
            new Dialogue(true,
            new List<Sentence>()
            {
                new AnswerableSentence(0, "Carlos", "Hey Harry, you coming for lunch?",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Sure!", 1)),
                        new DialogueOption(new Sentence(-1, "Harry", "Hmm... nah, thanks. I think I'll keep " +
                        "working for a bit longer.", 4))
                    }),
                new Sentence(1, "Carlos", "Sweet beans!"),
                new Sentence(2, "Carlos", "Speaking of which... I could definitely go for a burrito!"),
                new Sentence(3, "Kenza", "Ohhh, I would love a burrito right now!",
                    new Action(() => AttendCurrentAppointment())),
                new ConditionalSentence(4, new Func<bool>(() => GameManager.GetInstance().pc.LunchesSkipped > 2), 6,
                    "Carlos", "Hmm, you've been skipping a lot of lunches lately..."),
                new AnswerableSentence(5, "Carlos", "Don't forget that you need a break every now and then, or you'll " +
                "be stuck at home with a burnout at some point.",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "You're right. I guess I should take a break.", 1)),
                        new DialogueOption(new Sentence(-1, "Harry", "Thanks for your concern, but I'll manage. " +
                        "I'm in a productive flow right now.", 6))
                    }),
                new Sentence(6, "Carlos", "Alright, suit yourself. See you in an hour!",
                    new Action(() => CancelCurrentAppointment()))
            }));

        dialogueStorage.Add(DialogueKey.WARNING_WORKING_TOO_MUCH,
            new Dialogue(false,
            new List<Sentence>()
            {
                new Sentence(0, "Barrington", "Hey Harrington, I appreciate your drive, but... you might want " +
                "to take a break every now and then."),
                new Sentence(1, "Harry", "Fair point. Thanks for the reminder.", new Action(() => EndConversation()))
            }));

        dialogueStorage.Add(DialogueKey.WARNING_WORKING_TOO_LITTLE,
            new Dialogue(true,
            new List<Sentence>()
            {
                new Sentence(0, "Barrington", "Hey Harrington, please keep the amount of work in mind. " +
                "Deadlines are tight on this project."),
                new Sentence(1, "Harry", "Oh, right. Sorry.", new Action(() => EndConversation()))
            }));

        dialogueStorage.Add(DialogueKey.HARRY_IS_ASKED_FOR_HELP,
            new Dialogue(true,
            new List<Sentence>()
            {
                new AnswerableSentence(0, "Kenza", "Hey Harry, you got a sec?",
                    new List<DialogueOption>()
                    {
                        new DialogueOption(new Sentence(-1, "Harry", "Hey Kenz. Sure, what can I help you with?", 1)),
                        new DialogueOption(new Sentence(-1, "Harry", "Sorry, I'm a bit busy now.", 4))
                    }),
                new Sentence(1, "Kenza", "Great! So, I've been struggling with this issue for a minute now...",
                    new Action(() => AttendCurrentAppointment())),
                new Sentence(2, "Kenza", "Thanks again, Harry!"),
                new Sentence(3, "Harry", "No problem."),
                new Sentence(4, "Kenza", "Oh, okay.",
                    new Action(() => EndConversation()))
            }));
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
                return "Szabo";
        }
    }

    protected override bool CanBeClicked()
    {
        return true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    internal void LinkAppointmentToDialogue(string keyString, Appointment appointment)
    {
        GetDialogue(keyString).linkedAppointment = appointment;
    }

    private void AttendCurrentAppointment()
    {
        Appointment appointment = _currentDialogue.linkedAppointment;
        if (appointment != null)
        {
            appointment.Attend();
        }
    }

    private void CancelCurrentAppointment()
    {
        Appointment appointment = _currentDialogue.linkedAppointment;
        if (appointment != null)
        {
            appointment.Cancel();
        }
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

    private Dialogue GetDialogue(string keyAsString)
    {
        if (Enum.TryParse(keyAsString, out DialogueKey key))
        {
            return instance.dialogueStorage[key];
        }

        Debug.LogError("DialogueManager.GetDialogue(string): DialogueKey [" + keyAsString + "] not found.");
        return null;
    }

    public void StartConversation(string keyAsString)
    {
        if(Enum.TryParse(keyAsString, out DialogueKey key))
        {
            instance.StartConversation(key);
        }
        else
        {
            Debug.LogError("DialogueManager.StartDialogue(string): DialogueKey [" + keyAsString + "] not found.");
        }
    }

    public void StartConversation(DialogueKey key)
    {
        Dialogue dialogue = dialogueStorage[key];
        if (!(dialogue.WasHeld && dialogue.isOncePerDay))
        {
            _dialogueFilter.gameObject.SetActive(true);

            GameManager.GetInstance().CurrentState = GameManager.State.DIALOGUE;
            gameObject.SetActive(true);
            foreach (OptionButton button in _playerSideOptionButtons)
            {
                button.gameObject.SetActive(false);
            }
            foreach (OptionButton button in _otherSideOptionButtons)
            {
                button.gameObject.SetActive(false);
            }

            _currentDialogue = dialogueStorage[key];
            _currentSentenceIndex = 0;

            ShowSentence(_currentDialogue.sentences[_currentSentenceIndex]);
        }
    }

    internal void PreviewDialogueOption(int buttonIndex)
    {
        if (_currentSentence is AnswerableSentence)
        {
            _playerSide.gameObject.SetActive(true);
            DialogueOption selectedOption = ((AnswerableSentence)_currentSentence).OptionSelected(buttonIndex);
            _playerSideText.text = selectedOption.sentence.text;
            foreach (OptionButton button in _playerSideOptionButtons)
            {
                button.gameObject.SetActive(button.buttonIndex == buttonIndex);
            }

            _otherSide.alpha = 0.1f;
        }
    }

    internal void HideDialogueOption()
    {
        if (_currentSentence is AnswerableSentence)
        {
            _playerSide.gameObject.SetActive(false);
            _otherSide.alpha = 1;
        }
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
        _otherSide.alpha = 1;
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
                    typeSpeed = 0.5f;
                    break;
                case '?':
                    typeSpeed = 0.5f;
                    break;
                case '!':
                    typeSpeed = 0.5f;
                    break;
                case ',':
                    typeSpeed = 0.2f;
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
                _otherSideOptionButtons[i].gameObject.SetActive(true);
            }
        }
    }

    internal void OptionSelected(int optionIndex)
    {
        foreach (OptionButton button in _playerSideOptionButtons)
        {
            button.gameObject.SetActive(false);
        }
        foreach (OptionButton button in _otherSideOptionButtons)
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

    public void EndConversation()
    {
        _currentDialogue.WasHeld = true;

        _animator.Play("ConversationBar_FadeOut");
        GameManager.GetInstance().CurrentState = GameManager.State.FREE;
        _dialogueFilter.gameObject.SetActive(false);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
