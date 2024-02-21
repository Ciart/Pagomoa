using Ciart.Pagomoa.Systems;
using Logger;
using UnityEngine;

namespace Ciart.Pagomoa.Logger
{
    public class QuestEnroll : MonoBehaviour
    {
        private InteractableObject _interactableObject;
        [SerializeField] private int questId;

        private void Start()
        {
            if (questId == 0) questId = 1;
            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.InteractionEvent.AddListener(Enroll);
        }

        private void Enroll()
        {
            Debug.Log("퀘스트 등록");
            QuestManager.Instance.MakeQuest(questId);
        }
    }
}
