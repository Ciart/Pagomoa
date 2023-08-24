using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    public List<Dialogue> dialogues;

    [Space]
    public GameObject talkPannel;
    public Image talkImage;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    [Space]
    public Dialogue NowScenario;
    int talkIndex;
    [Space] 
    public PlayableDirector director;


    private static DialogueManager _instance = null;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
            }
            return _instance;
        }
    }
    Dialogue GetDialogueByID(int id)
    {
        foreach(Dialogue dialogue in dialogues)
        {
            if (dialogue.id == id)
                return dialogue;
        }
        return null;
    }
    public bool ConversationProgress(int id)
    {
        NowScenario = GetDialogueByID(id);

        if(NowScenario.talk.Count <= talkIndex)
        {
            talkIndex = 0;
            NowScenario = null;
            talkPannel.SetActive(false);
            return false;
        }
        talkText.text = NowScenario.talk[talkIndex];
        talkImage.sprite = NowScenario.sprite[talkIndex];
        nameText.text = NowScenario.talkerName[talkIndex];
        talkPannel.SetActive(true);
        talkIndex++;
        return true;
    }
}
