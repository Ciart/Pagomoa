using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Logger;
using UnityEngine;

namespace Ciart.Pagomoa.Logger
{
    public class QuestEnroll : MonoBehaviour
    {
        private InteractableObject _interactableObject;
        [SerializeField] private SpriteRenderer spriteRenderer; // todo 코드 변경을 생각해 봐야함
        [SerializeField] private int questId;

        private void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.InteractionEvent.AddListener(EnrollQuest);

            spriteRenderer ??= transform.GetChild(1).GetComponent<SpriteRenderer>();

            spriteRenderer.enabled = false;
        }

        private void EnrollQuest()
        {
            Debug.Log("퀘스트 등록");
            EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
            QuestManager.instance.MakeQuest(this, questId);
            _interactableObject.InteractionEvent.RemoveListener(EnrollQuest);
        }

        private void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;
            
            spriteRenderer.enabled = isQuestComplete;
            
            // 끝났음 퀘스트 보상 받을 준비같은거 해야함
        }
    }
}
