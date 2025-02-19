using System.Collections.Generic;
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
        [SerializeField] private List<string> auctionItemsIDs = new();

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.interactionEvent.AddListener(SetUI);
        }
        public void SetUI()
        {
            var shopUI = Game.Instance.UI.shopUI;
            shopUI.GetShopChat().AwakeChat();
            shopUI.SetShopItemIDs(auctionItemsIDs);
        }
        public void OffUI()
        {
            Game.Instance.Time.ResumeTime();
            Game.Instance.UI.shopUI.gameObject.SetActive(false);
        }
    }
}
