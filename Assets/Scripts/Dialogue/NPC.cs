using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int dialogueId;
    bool isTalking = false; 
    public void Talking()
    {
        if (!isTalking)
        {
            GetComponent<Animator>().SetTrigger("StartTalk");
            //Debug.Log("대화 시작시 애니메이션 진행이 필요하다면 Trigger StartTalk추가 후 발동 시켜주세요.");
            if (GetComponent<AutoChat>())
                GetComponent<AutoChat>().StopChat();
            isTalking = true;
        }
        if (!DialogueManager.Instance.ConversationProgress(dialogueId))
            StopTalking();
            
    }
    public void StopTalking()
    {
        if (GetComponent<AutoChat>())
            GetComponent<AutoChat>().StartChatReservation();
        isTalking = false;
    }


}
