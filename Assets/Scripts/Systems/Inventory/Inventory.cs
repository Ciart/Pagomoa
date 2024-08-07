using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class Inventory
    {
        public int Gold;
        [SerializeField] public int stoneCount;
        [SerializeField] public int maxCount;
        
        public const int MaxQuickItems = 6;
        private Item[] _quickItems = new Item[MaxQuickItems];
        
        public const int MaxArtifactItems = 4;
        public InventorySlot[] artifactItems = new InventorySlot[MaxArtifactItems];
        
        public const int MaxItems = 36;
        public InventorySlot[] items = new InventorySlot[MaxItems];
        
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
                InventorySlot item = items[idx];
                
                if (item.count == 0)
                    DecreaseItemCount(data);
            }
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
                ref var item = ref items[idx];
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
                if (items[idx].count > 1)
                {
                    items[idx].count--;
                }
                else if (items[idx].count == 1 || items[idx].count == 0)
                {
                    items[idx].item = null;
                    items[idx].count = 0;
                }
                
                EventManager.Notify(new ItemCountChangedEvent(data, items[idx].count));
            }
        }
        
        public void RemoveItemData(Item data)
        {
            var idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx == -1) return;
            
            items[idx].item = null;
            items[idx].count = 0;
            
            EventManager.Notify(new ItemCountChangedEvent(items[idx].item, items[idx].count));
        }

        public void SwapSlot(int a, int b)
        {
            (items[a], items[b]) = (items[b], items[a]);
            EventManager.Notify(new ItemCountChangedEvent(items[a].item, items[a].count));
            EventManager.Notify(new ItemCountChangedEvent(items[b].item, items[b].count));
        }
        
        public int GetItemCount(Item data)
        {
            var idx = Array.FindIndex(items, element => element.item == data);

            if (idx != -1)
                return items[idx].count;
            
            return 0;
        }
        
        public Item GetQuickItem(int id)
        {
            return _quickItems[id];
        }
        
        public void SetQuickItem(int id, Item item)
        {
            _quickItems[id] = item;
            EventManager.Notify(new QuickSlotChangedEvent(_quickItems));
        }
        
        public void SwapQuickSlot(int a, int b)
        {
            (_quickItems[a], _quickItems[b]) = (_quickItems[b], _quickItems[a]);
            EventManager.Notify(new QuickSlotChangedEvent(_quickItems));
        }
        
        // TODO: GameManager.player.GetComponent<PlayerStatus>() 없애야 합니다. Item.Active()에 왜 stat을 넣어야 하나요?
        public void UseQuickSlotItem(int index)
        {
            var item = _quickItems[index];
            
            switch (item.itemType)
            {
                case Item.ItemType.Use:
                    if (GameManager.player.inventory.GetItemCount(item) != 0)
                    {
                        DecreaseItemCount(item);
                        item.Active(GameManager.player.GetComponent<PlayerStatus>());
                    }
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
