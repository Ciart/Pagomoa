using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Logger;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Logger
{
    public class QuestEnroll : MonoBehaviour
    {
        private InteractableObject _interactableObject;
        [SerializeField] private SpriteRenderer questFinishRenderer;

        private void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.interactionEvent.AddListener(EnrollQuest);

            questFinishRenderer ??= transform.GetChild(1).GetComponent<SpriteRenderer>();
            questFinishRenderer.enabled = false;
        }

        private void EnrollQuest()
        {
            Debug.Log("퀘스트 등록");
            EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
            //QuestManager.instance.RegistrationQuest(this, questId);
            _interactableObject.interactionEvent.RemoveListener(EnrollQuest);

            if (QuestUI.instance != null)
            {
                QuestUI.instance.npcImages.Add(GetComponent<SpriteRenderer>().sprite);
                QuestUI.instance.MakeProgressQuestList();
            }
            else
                return;
        }

        private void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;

            if (isQuestComplete == false)
            {
                   
            }
            else
            {
                questFinishRenderer.gameObject.SetActive(true);
            }

            // 끝났음 퀘스트 보상 받을 준비같은거 해야함
        }

        private void GetReward()
        {
            
        }
    }
}
