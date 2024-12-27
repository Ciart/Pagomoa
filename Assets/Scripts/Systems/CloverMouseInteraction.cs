using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class CloverMouseInteraction : MonoBehaviour
    {
        private InteractableObject _interactable;

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.interactionEvent.AddListener(SetUI);
        }
        public void SetUI()
        {
            var timeManager = TimeManager.instance;
            var shopUI = UIManager.instance.shopUI;
            
            shopUI.gameObject.SetActive(true);
            timeManager.PauseTime();
            shopUI.GetShopChat().AwakeChat();
        }
        public void OffUI()
        {
            var timeManager = TimeManager.instance;
            var shopUI = UIManager.instance.shopUI;
            
            timeManager.ResumeTime();
            shopUI.gameObject.SetActive(false);
        }
    }
}
