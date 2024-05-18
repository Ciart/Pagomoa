using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Logger
{
    public class QuestCompleteIcon : MonoBehaviour
    {
        private int _siblingIndex;
        private SpriteRenderer _finishIcon;
        
        private void Awake()
        {
            _finishIcon = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }
        
        public void ActiveQuestComplete() { gameObject.SetActive(true); }
        
        public void InactiveQuestComplete() { gameObject.SetActive(false); }

        public int GetIconIndex() { return _siblingIndex; }
    }
}
