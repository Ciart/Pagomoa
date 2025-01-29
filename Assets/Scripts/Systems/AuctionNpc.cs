using System.Linq;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class AuctionNpc : MonoBehaviour
    {
        private InteractableObject _interactable;
        private Auction _auction;

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.interactionEvent.AddListener(SetUI);
            
            _auction = GetComponent<Auction>();
        }
        public void SetUI()
        {
            var shopUI = UIManager.instance.shopUI;
            shopUI.GetShopChat().AwakeChat();
            shopUI.SetShopItemIDs(_auction.GetAuctionItemIDs());
            
            shopUI.gameObject.SetActive(true);
            
            var timeManager = TimeManager.instance;
            timeManager.PauseTime();
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
