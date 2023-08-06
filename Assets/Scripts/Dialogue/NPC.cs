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
            //Debug.Log("��ȭ ���۽� �ִϸ��̼� ������ �ʿ��ϴٸ� Trigger StartTalk�߰� �� �ߵ� �����ּ���.");
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
