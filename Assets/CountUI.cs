using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class CountUI : MonoBehaviour
    {
        public TextMeshProUGUI countUIText;
        public int inputCount;
        
        private ISlot _chosenSlot;
        
        [Header("아이템 구매&판매 제어 버튼")]
        public Button countUpButton;
        public Button countDownButton;
        [SerializeField] private Button buyYesButton;
        [SerializeField] private Button buyNoButton;
        [SerializeField] private Button sellYesButton;
        [SerializeField] private Button sellNoButton;
        
        public void ActiveCountUI(ISlot slot)
        {
            _chosenSlot = slot;
            
            countUpButton.gameObject.SetActive(true);
            countDownButton.gameObject.SetActive(true);
            countUpButton.onClick.RemoveAllListeners();
            countDownButton.onClick.RemoveAllListeners();
            
            if (_chosenSlot.GetSlotType() == SlotType.Buy || _chosenSlot.GetSlotType() == SlotType.BuyArtifact)
            {
                countUpButton.onClick.AddListener(BuyPlus);
                countDownButton.onClick.AddListener(BuyMinus);
                buyYesButton.gameObject.SetActive(true);
                buyNoButton.gameObject.SetActive(true);
            }
            else if (_chosenSlot.GetSlotType() == SlotType.Sell)
            {
                countUpButton.onClick.AddListener(SellPlus);
                countDownButton.onClick.AddListener(SellMinus);
                sellYesButton.gameObject.SetActive(true);
                sellNoButton.gameObject.SetActive(true);
            }

            inputCount = 1;
            countUIText.text = inputCount.ToString();
        }
        
        
        public void DisableCountUI()
        {
            var shopUI = Game.Instance.UI.shopUI;
            shopUI.GetShopChat().CancelChat();
            
            if (_chosenSlot.GetSlotType() == SlotType.Buy)
            {
                countUpButton.onClick.RemoveAllListeners();
                countDownButton.onClick.RemoveAllListeners();
            }
            else if (_chosenSlot.GetSlotType() == SlotType.Sell)
            {
                countUpButton.onClick.RemoveAllListeners();
                countDownButton.onClick.RemoveAllListeners();
            }

            countUpButton.gameObject.SetActive(false);
            countDownButton.gameObject.SetActive(false);
            buyYesButton.gameObject.SetActive(false);
            buyNoButton.gameObject.SetActive(false);
            sellYesButton.gameObject.SetActive(false);
            sellNoButton.gameObject.SetActive(false);
            
            gameObject.SetActive(false);
        }
        
        public void BuyPlus()
        {
            var slotID = _chosenSlot.GetSlotID();
            var shopUI = Game.Instance.UI.shopUI;
            var item = ResourceSystem.instance.GetItem(shopUI.GetShopItemIDs()[slotID]);
            
            if (item.type == ItemType.Use)
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * item.price);
            }
            
            countUIText.text = inputCount.ToString();
        }
        
        public void BuyMinus()
        {
            var slotID = _chosenSlot.GetSlotID();
            var shopUI = Game.Instance.UI.shopUI;
            var item = ResourceSystem.instance.GetItem(shopUI.GetShopItemIDs()[slotID]);
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * item.price);
            }
            
            countUIText.text = inputCount.ToString();
        }
        public void BuySlots()
        {
            var inventory = Game.Instance.player.inventory;
            var slotID = _chosenSlot.GetSlotID();
            var shopUI = Game.Instance.UI.shopUI;
            var item = ResourceSystem.instance.GetItem(shopUI.GetShopItemIDs()[slotID]);

            if (item.type == ItemType.Use)
            {
                var totalPrice = item.price * inputCount;
                
                if (inventory.gold >= totalPrice && inputCount > 0)
                {
                    inventory.AddInventory(item.id, inputCount);
                    DisableCountUI();
                    
                    inventory.gold -= totalPrice;
                    Game.Instance.UI.UpdateGoldUI();
                }
                else return;
                
                shopUI.GetShopChat().ThankChat();
            }
            else if (item.type == ItemType.Equipment || item.type == ItemType.Inherent)
            {
                var totalPrice = item.price * inputCount;
                
                if (inventory.gold >= totalPrice && inputCount == 1)
                {
                    inventory.AddInventory(item.id);
                    DisableCountUI();
                    
                    inventory.gold -= totalPrice;
                    Game.Instance.UI.UpdateGoldUI();
                    Game.Instance.UI.shopUI.GetBuyUI().SoldOut(slotID);
                }
                else
                    return;
                
                shopUI.GetShopChat().ThankChat();
            }
        }
        
        public void SellPlus()
        {
            var slotID = _chosenSlot.GetSlotID();
            var shopUI = Game.Instance.UI.shopUI;
            var inventorySlot = Game.Instance.player.inventory.FindSlot(SlotType.Inventory, slotID);
            
            if (inputCount < inventorySlot.GetSlotItemCount())
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * inventorySlot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellMinus()
        {
            var slotID = _chosenSlot.GetSlotID();
            var shopUI = Game.Instance.UI.shopUI;
            var inventorySlot = Game.Instance.player.inventory.FindSlot(SlotType.Inventory, slotID);
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * inventorySlot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellSlots()
        {
            var slotID = _chosenSlot.GetSlotID();
            var inventory = Game.Instance.player.inventory;
            var shopUI = Game.Instance.UI.shopUI;
            var inventorySlot = inventory.FindSlot(SlotType.Inventory, slotID);
            
            for (int i = 0; i < inputCount; i++)
            {
                if (inventorySlot.GetSlotItemCount() > 0)
                {
                    inventory.SellItem(_chosenSlot);
                }
                
                Game.Instance.UI.shopUI.GetSellUI().ClearSellUISlot();
                Game.Instance.UI.shopUI.GetSellUI().UpdateSellUISlot();
            }
            
            if (inventorySlot.GetSlotItemCount() == 0)
            {
                var sellSlot = (SellSlotUI)_chosenSlot;
                var emptySlot = new Slot();
                emptySlot.SetSlotItemID("");
                sellSlot.SetSlot(emptySlot);
            }
            
            inputCount = 1;
            countUIText.text = inputCount.ToString();
            
            DisableCountUI();
            shopUI.GetShopChat().ThankChat();
        }
    }
}
