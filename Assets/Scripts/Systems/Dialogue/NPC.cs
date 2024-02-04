using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public bool isRandomTalk = false;
    public List<int> dialogueId = new List<int>();
    public UnityEvent AllDialogueReadEvent;

    bool isTalking = false;

    bool nowTalkable = true;
    int nowTalkDialogueId;
    int iter = 0;

    private void Awake()
    {
        nowTalkDialogueId = dialogueId[iter];
    }
    public void Talking()
    {
        if (!nowTalkable) return;
        if (!isTalking)
        {
            GetComponent<Animator>().SetTrigger("StartTalk");
            //Debug.Log("대화 시작시 애니메이션 진행이 필요하다면 Trigger StartTalk추가 후 발동 시켜주세요.");
            if (GetComponent<AutoChat>())
                GetComponent<AutoChat>().StopChat();
            isTalking = true;
        }

        if (!DialogueManager.Instance.ConversationProgress(nowTalkDialogueId))
        {
            StopTalking();
            ChangeTalkDialogueId();
        }
            
    }
    void ChangeTalkDialogueId()
    {
        if (isRandomTalk)
        {
            dialogueId.Sort();
            iter = Random.Range(0, dialogueId.Count);
            nowTalkDialogueId = dialogueId[iter];
        }
        else
        {
            if (iter < dialogueId.Count - 1)
            {
                iter++;
                nowTalkDialogueId = dialogueId[iter];
            }
            else
            {
                iter = 0;
                nowTalkDialogueId = dialogueId[iter];
                if (AllDialogueReadEvent != null)
                    AllDialogueReadEvent.Invoke();
            }
        }
    }
    public void SetNowTalkable(bool tf) { nowTalkable = tf; }
    public void StopTalking()
    {
        if (GetComponent<AutoChat>())
            GetComponent<AutoChat>().StartChatReservation();
        isTalking = false;
    }


}
