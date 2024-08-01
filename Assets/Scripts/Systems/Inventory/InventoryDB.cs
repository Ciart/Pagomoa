using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class InventoryDB
    {
        public int Gold;
        [SerializeField] public int stoneCount;
        [SerializeField] public int maxCount;
        
        public const int MaxQuickSlot = 6;
        private InventoryItem[] _quickSlots = new InventoryItem[MaxQuickSlot];
        
        public const int MaxArtifactItems = 4;
        public InventoryItem[] artifactItems = new InventoryItem[MaxArtifactItems];
        
        public const int MaxItems = 36;
        public InventoryItem[] items = new InventoryItem[MaxItems];
        
        private void Start()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
        }
        
        private void ChangeGold(AddGold e)
        {
            AddGold(e.gold);
        }
        private void AddReward(AddReward e)
        {
            Add(e.item, e.itemCount);
        }
        
        private void AddGold(int gold)
        {
            this.Gold += gold;
        }
        public void Equip(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];
                
                if (item.count == 0)
                    DecreaseItemCount(data);
            }
            InventoryUI.Instance.ResetSlot();
        }
        
        public void AddArtifactData(Item data)
        {
            for (int i = 0; i < artifactItems.Length; i++)
            {
                if (artifactItems[i].item == null)
                {
                    artifactItems[i].item = data;
                    break;
                }
                // else if(artifactItems[artifactItems.Length].item != null)
                // {
                //     Debug.Log("꽉참");
                //     return;
                // } 수정 필요
            }
        }
        public void RemoveArtifactData(Item data)
        {
            int idx = Array.FindIndex(artifactItems, element => element.item == data);
            
            if (idx != -1)
            {
                artifactItems[idx].item = null;
            }
        }
        
        public void Add(Item data, int count = 1) // Item data
        {
            int idx = Array.FindIndex(items, element => element.item == data);

            if (idx != -1)
            {
                var item = items[idx];
                item.count += count;
                
                EventManager.Notify(new ItemCountChangedEvent(item.item, item.count));
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == null)
                    {
                        items[i].item = data;
                        items[i].count = count;
                        EventManager.Notify(new ItemCountChangedEvent(items[i].item, items[i].count));
                        break;
                    }
                }
            }
            if (InventoryUI.Instance)
                InventoryUI.Instance.ResetSlot();
        }
        public void SellItem(Item data)
        {
            DecreaseItemCount(data);
            Gold += data.itemPrice;
            ShopUIManager.Instance.gold[0].text = Gold.ToString();
            ShopUIManager.Instance.gold[1].text = Gold.ToString();
        }
        public void DecreaseItemCount(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];
                
                if (item.count > 1)
                {
                    item.count--;
                }
                else if (item.count == 1 || item.count == 0)
                {
                    item.item = null;
                    item.count = 0;
                }
                
                EventManager.Notify(new ItemCountChangedEvent(data, item.count));
            }
        }
        public void RemoveItemData(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == item)
                    {
                        items[i].item = null;
                        items[i].count = 0;

                        EventManager.Notify(new ItemCountChangedEvent(item.item, items[i].count));
                    }
                }
            }
        }
        public int GetItemCount(Item data)
        {
            var idx = Array.FindIndex(items, element => element.item == data);

            if (idx != -1)
                return items[idx].count;
            
            return 0;
        }
        
        public Item GetQuickSlot(int id)
        {
            return _quickSlots[id].item;
        }
        
        public void SetQuickSlot(int id, Item item)
        {
            _quickSlots[id].item = item;
            EventManager.Notify(new QuickSlotChangedEvent(_quickSlots));
        }
        
        public void SwapQuickSlot(int a, int b)
        {
            (_quickSlots[a], _quickSlots[b]) = (_quickSlots[b], _quickSlots[a]);
            EventManager.Notify(new QuickSlotChangedEvent(_quickSlots));
        }
        
        // TODO: GameManager.player.GetComponent<PlayerStatus>() 없애야 합니다. Item.Active()에 왜 stat을 넣어야 하나요?
        public void UseQuickSlotItem(int index)
        {
            var item = _quickSlots[index].item;
            
            switch (item.itemType)
            {
                case Item.ItemType.Use:
                    DecreaseItemCount(item);
                    item.Active(GameManager.player.GetComponent<PlayerStatus>());
                    break;
                case Item.ItemType.Inherent:
                    item.Active(GameManager.player.GetComponent<PlayerStatus>());
                    break;
                default:
                    return;
            }
        }
    }
}
