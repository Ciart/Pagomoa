using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Logger
{
    public class QuestCompleteIcon : MonoBehaviour
    {
        public int questId;

        private int _siblingIndex;
        private SpriteRenderer _finishIcon;
        
        private void Awake()
        {
            _finishIcon = GetComponent<SpriteRenderer>();
            _finishIcon.gameObject.SetActive(false);

            _siblingIndex = transform.GetSiblingIndex();
        }
        
        public void ActiveQuestComplete() { _finishIcon.gameObject.SetActive(true); }
        
        public void InactiveQuestComplete() { _finishIcon.gameObject.SetActive(false); }

        public int GetIconIndex() { return _siblingIndex; }

        private void OnDestroy()
        {
            
        }
    }
}
