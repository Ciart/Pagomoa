using System.Collections;
using System.Collections.Generic;
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
        
        private SlotType _chosenSlotType;
        
        [Header("아이템 구매&판매 제어 버튼")]
        public Button countUpButton;
        public Button countDownButton;
        [SerializeField] private Button buyYesButton;
        [SerializeField] private Button buyNoButton;
        [SerializeField] private Button sellYesButton;
        [SerializeField] private Button sellNoButton;
        
        public void ActiveCountUI(SlotType type)
        {
            _chosenSlotType = type;
            
            countUpButton.gameObject.SetActive(true);
            countDownButton.gameObject.SetActive(true);
            countUpButton.onClick.RemoveAllListeners();
            countDownButton.onClick.RemoveAllListeners();
            
            if (type == SlotType.Buy || type == SlotType.BuyArtifact)
            {
                countUpButton.onClick.AddListener(BuyPlus);
                countDownButton.onClick.AddListener(BuyMinus);
                buyYesButton.gameObject.SetActive(true);
                buyNoButton.gameObject.SetActive(true);
            }
            else if (type == SlotType.Sell)
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
            var shopUI = UIManager.instance.shopUI;
            shopUI.GetShopChat().CancelChat();
            
            if (_chosenSlotType == SlotType.Buy)
            {
                countUpButton.onClick.RemoveAllListeners();
                countDownButton.onClick.RemoveAllListeners();
            }
            else if (_chosenSlotType == SlotType.Sell)
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
            var shopUI = UIManager.instance.shopUI;
            var buySlot = (BuySlot)UIManager.instance.GetUIContainer().chosenSlot;
            
            if (buySlot.GetSlotType() != SlotType.Buy)
                if (buySlot.GetSlotType() != SlotType.BuyArtifact)
                    return;
            
            if (buySlot.slot.GetSlotItem().type == ItemType.Use)
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.slot.GetSlotItem().price);
            }
            else if (buySlot.slot.GetSlotItem().type == ItemType.Equipment || 
                     buySlot.slot.GetSlotItem().type == ItemType.Inherent)
            {
                if (inputCount < buySlot.slot.GetSlotItemCount())
                    inputCount++;
            }
            
            countUIText.text = inputCount.ToString();
        }
        
        public void BuyMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var buySlot = (BuySlot)UIManager.instance.GetUIContainer().chosenSlot;
            
            if (buySlot.GetSlotType() != SlotType.Buy)
                if (buySlot.GetSlotType() != SlotType.BuyArtifact)
                    return;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.slot.GetSlotItem().price);
            }
            
            countUIText.text = inputCount.ToString();
        }
        public void BuySlots()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var buySlot = (BuySlot)UIManager.instance.GetUIContainer().chosenSlot;

            if (buySlot.GetSlotType() != SlotType.Buy)
                if (buySlot.GetSlotType() != SlotType.BuyArtifact)
                    return;


            if (buySlot.slot.GetSlotItem().type == ItemType.Use)
            {
                var totalPrice = buySlot.slot.GetSlotItem().price * inputCount;
                
                if (inventory.gold >= totalPrice && inputCount > 0)
                {
                    inventory.Add(buySlot.slot.GetSlotItem(), inputCount);
                    shopUI.GetShopItems().Remove(buySlot.slot.GetSlotItem());
                    DisableCountUI();
                    
                    inventory.gold -= totalPrice;
                    UIManager.instance.UpdateGoldUI();
                }
                else return;
                
                shopUI.GetShopChat().ThankChat();
            }
            else if (buySlot.slot.GetSlotItem().type == ItemType.Equipment || 
                     buySlot.slot.GetSlotItem().type == ItemType.Inherent)
            {
                var totalPrice = buySlot.slot.GetSlotItem().price * inputCount;
                
                if (inventory.gold >= totalPrice && buySlot.slot.GetSlotItemCount() == inputCount)
                {
                    inventory.Add(buySlot.slot.GetSlotItem());
                    shopUI.GetShopItems().Remove(buySlot.slot.GetSlotItem());
                    DisableCountUI();
                    
                    inventory.gold -= totalPrice;
                    UIManager.instance.UpdateGoldUI();
                    UIManager.instance.shopUI.GetBuyUI().SoldOut();
                }
                else
                    return;
                
                shopUI.GetShopChat().ThankChat();
            }
        }
        
        public void SellPlus()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = (SellSlot)UIManager.instance.GetUIContainer().chosenSlot;
            
            if(sellSlot.GetSlotType() != SlotType.Sell)
                return;
            
            if (inputCount < sellSlot.slot.GetSlotItemCount())
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.slot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = (SellSlot)UIManager.instance.GetUIContainer().chosenSlot;
            
            if(sellSlot.GetSlotType() != SlotType.Sell)
                return;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.slot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = (SellSlot)UIManager.instance.GetUIContainer().chosenSlot;
            
            if(sellSlot.GetSlotType() != SlotType.Sell)
                return;
            
            for (int i = 0; i < inputCount; i++)
            {
                if (sellSlot.slot.GetSlotItemCount() > 0)
                {
                    inventory.SellItem(sellSlot);
                }
                
                UIManager.instance.shopUI.GetSellUI().DeleteSellUISlot();
                UIManager.instance.shopUI.GetSellUI().UpdateSellUISlot();
            }
            
            if (sellSlot.slot.GetSlotItemCount() == 0)
            {
                sellSlot.ResetSlot();
            }
            
            inputCount = 1;
            countUIText.text = inputCount.ToString();
            
            DisableCountUI();
            shopUI.GetShopChat().ThankChat();
        }
    }
}
