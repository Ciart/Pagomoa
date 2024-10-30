using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class CloverMouseInteraction : MonoBehaviour
    {
        private InteractableObject _interactable;
        [SerializeField] private GameObject _buyUI;

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.interactionEvent.AddListener(SetUI);
        }
        private void SetUI()
        {
            var timeManager = TimeManager.instance;
            
            if (_buyUI.activeSelf == false)
            {
                _buyUI.SetActive(true);
                timeManager.PauseTime();
                ShopChat.Instance.AwakeChat();
            }
            else
            {
                OffUI();
            }
        }
        public void OffUI()
        {
            var timeManager = TimeManager.instance;
            
            timeManager.ResumeTime();
            _buyUI.SetActive(false);
        }
    }
}
