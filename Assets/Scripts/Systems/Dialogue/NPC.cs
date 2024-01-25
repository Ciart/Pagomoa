using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class NPC : MonoBehaviour
    {
        public bool isRandomTalk = false;
        public List<int> dialogueId = new List<int>();
        public UnityEvent AllDialogueReadEvent;

        private bool isTalking = false;

        private bool nowTalkable = true;
        private int nowTalkDialogueId;
        private int iter = 0;

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

        private void ChangeTalkDialogueId()
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
}
