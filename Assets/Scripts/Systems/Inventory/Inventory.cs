using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public partial class Inventory : MonoBehaviour
    {
        public int gold;

        private Slot[] _quickData = new Slot[MaxQuickSlots];
        public const int MaxQuickSlots = 6;

        private Slot[] _artifactSlots = new Slot[MaxArtifactSlots];
        public const int MaxArtifactSlots = 4;

        private Slot[] _inventoryData = new Slot[MaxInventorySlots];
        public const int MaxInventorySlots = 36;
        public const int MaxUseItemCount = 64;
        public const int MaxInherentItemCount = 1;

        public Action artifactChanged;

        public InventorySaveData CreateSaveData()
        {
            var savaData = new InventorySaveData();

            savaData.quickSlots = new InventorySlotSaveData[MaxQuickSlots];
            savaData.artifactSlots = new InventorySlotSaveData[MaxArtifactSlots];
            savaData.inventorySlots = new InventorySlotSaveData[MaxInventorySlots];

            for (var i = 0; i < MaxQuickSlots; i++)
            {
                savaData.quickSlots[i] = new InventorySlotSaveData
                {
                    id = _quickData[i].GetSlotItemID(),
                    count = _quickData[i].GetSlotItemCount()
                };
            }

            for (var i = 0; i < MaxArtifactSlots; i++)
            {
                savaData.artifactSlots[i] = new InventorySlotSaveData
                {
                    id = _artifactSlots[i].GetSlotItemID(),
                    count = _artifactSlots[i].GetSlotItemCount()
                };
            }

            for (var i = 0; i < MaxInventorySlots; i++)
            {
                savaData.inventorySlots[i] = new InventorySlotSaveData
                {
                    id = _inventoryData[i].GetSlotItemID(),
                    count = _inventoryData[i].GetSlotItemCount()
                };
            }

            return savaData;
        }

        public void ApplySaveData(InventorySaveData saveData)
        {
            for (var i = 0; i < MaxQuickSlots; i++)
            {
                _quickData[i].SetSlotItemID(saveData.quickSlots[i].id);
                _quickData[i].SetSlotItemCount(saveData.quickSlots[i].count);
            }

            for (var i = 0; i < MaxArtifactSlots; i++)
            {
                _artifactSlots[i].SetSlotItemID(saveData.artifactSlots[i].id);
                _artifactSlots[i].SetSlotItemCount(saveData.artifactSlots[i].count);
            }

            for (var i = 0; i < MaxInventorySlots; i++)
            {
                _inventoryData[i].SetSlotItemID(saveData.inventorySlots[i].id);
                _inventoryData[i].SetSlotItemCount(saveData.inventorySlots[i].count);
            }
            
            EventManager.Notify(new LoadInventoryEvent(this));
            EventManager.Notify(new UpdateInventoryEvent(this));
            artifactChanged?.Invoke();
        }

        private void OnEnable()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<AddReward>(AddReward);
            EventManager.RemoveListener<AddGold>(ChangeGold);
        }

        private void Awake()
        {
            InitSlots();
        }

        // 초기 인벤토리 초기화
        public void InitSlots()
        {
            for (int i = 0; i < MaxQuickSlots; i++)
            {
                _quickData[i] = new Slot();
                _quickData[i].SetSlotType(SlotType.Quick);
            }

            for (int i = 0; i < MaxArtifactSlots; i++)
            {
                _artifactSlots[i] = new Slot();
                _artifactSlots[i].SetSlotType(SlotType.Artifact);
            }

            for (int i = 0; i < MaxInventorySlots; i++)
            {
                _inventoryData[i] = new Slot();
                _inventoryData[i].SetSlotType(SlotType.Inventory);
            }
        }

        private void ChangeGold(AddGold e) { AddGold(e.gold); }
        private void AddReward(AddReward e) { AddInventory(e.itemID, e.itemCount); }
        private void AddGold(int addGold) { gold += addGold; }
    }

    #region 인벤토리 함수
    public partial class Inventory
    {
        public void AddInventory(string itemID, int itemCount = 1)
        {
            var hasItemList = FindSameItem(itemID);
            
            if (hasItemList.Count > 0)
            {
                var accCount = itemCount;
                foreach (var idx in hasItemList)
                {
                    accCount += _inventoryData[idx].GetSlotItemCount();
                }

                var divVal = accCount / MaxUseItemCount;
                var remainVal = accCount % MaxUseItemCount;
                var emptySlotCount = remainVal > 0 ? divVal + 1 : divVal;
                emptySlotCount -= hasItemList.Count;
                
                if (emptySlotCount > 0)
                {
                    for(var i = 0; i < hasItemList.Count; i++)
                        _inventoryData[hasItemList[i]].SetSlotItemCount(MaxUseItemCount);
                    for (var i = 0; i < divVal - hasItemList.Count; i++)
                    {
                        var emptySlot = FindSlotByItemID(SlotType.Inventory, "");
                        if (emptySlot == null)
                        {
                            Debug.Log($"{itemID}가 {MaxUseItemCount}개 버려졌습니다.");
                            continue;
                        }
                        emptySlot.SetSlotItemID(itemID);
                        emptySlot.SetSlotItemCount(MaxUseItemCount);
                    }
                    if (remainVal > 0)
                    {
                        var emptySlot = FindSlotByItemID(SlotType.Inventory, "");
                        if (emptySlot == null)
                        {
                            Debug.Log($"{itemID}가 {remainVal}개 버려졌습니다.");
                        }
                        else
                        {
                            emptySlot.SetSlotItemID(itemID);
                            emptySlot.SetSlotItemCount(remainVal);
                        }
                    }
                }
                else
                {
                    for(var i = 0; i < divVal; i++)
                        _inventoryData[hasItemList[i]].SetSlotItemCount(MaxUseItemCount);
                    var lastIndex = hasItemList.Count - 1;
                    if (remainVal > 0)
                        _inventoryData[hasItemList[lastIndex]].SetSlotItemCount(remainVal);
                }
                // TODO : 사용 아이템 이외에는 복수 장착이 있는가?
            }
            else // 새로운 아이템을 획득했을 때
            {
                var divVal = itemCount / MaxUseItemCount;
                var remainVal = itemCount % MaxUseItemCount;
                var emptySlots = FindSameItem("");
                
                for (var i = 0; i < divVal; i++)
                {
                    if (emptySlots.Count - 1 < i)
                    {
                        Debug.Log($"{itemID}가 {MaxUseItemCount}개 버려졌습니다.");
                    }
                    else
                    {
                        _inventoryData[emptySlots[i]].SetSlotItemID(itemID);
                        _inventoryData[emptySlots[i]].SetSlotItemCount(MaxUseItemCount);   
                    }
                }
                if (remainVal > 0)
                {
                    var emptySlot = FindSlotByItemID(SlotType.Inventory, "");
                    if (emptySlot == null)
                    {
                        Debug.Log($"{itemID}가 {remainVal}개 버려졌습니다.");
                    }
                    else
                    {
                        emptySlot.SetSlotItemID(itemID);
                        emptySlot.SetSlotItemCount(remainVal);   
                    }
                }
            }

            var registrationSlot = FindSlotByItemID(SlotType.Quick, itemID);
            if (registrationSlot != null)
            {
                UpdateQuickSlot();
            }

            // TODO : 빈 슬롯 없을때는 어떻게 해야하노

            var sameItemList = FindSameItem(itemID);
            var accItemCount = 0;
            foreach (var slotID in sameItemList)
            {
                accItemCount += _inventoryData[slotID].GetSlotItemCount();
            }
            EventManager.Notify(new UpdateInventoryEvent(this));
            EventManager.Notify(new ItemCountChangedEvent(itemID, accItemCount));
        }

        public void SellItem(ISlot targetSlot)
        {
            var targetItemID = FindSlot(SlotType.Inventory, targetSlot.GetSlotID()).GetSlotItemID();
            EventManager.Notify(new ItemSellEvent(targetItemID, 1));

            gold += _inventoryData[targetSlot.GetSlotID()].GetSlotItem().price;
            DecreaseItemBySlotID(targetSlot.GetSlotID());

            Game.Instance.UI.UpdateGoldUI();
        }

        public void DecreaseItemBySlotID(int targetSlotID, int itemCount = 1)
        {
            var inventorySlot = _inventoryData[targetSlotID];
            var count = inventorySlot.GetSlotItemCount() - itemCount;
            var slotItemID = inventorySlot.GetSlotItemID();

            if (count >= 1)
            {
                _inventoryData[targetSlotID].SetSlotItemCount(count);
            }
            else if (count == 0)
            {
                _inventoryData[targetSlotID].SetSlotItemID("");
                _inventoryData[targetSlotID].SetSlotItemCount(0);
            }

            var registrationSlot = FindSlotByItemID(SlotType.Quick, inventorySlot.GetSlotItemID());
            if (registrationSlot != null)
            {
                UpdateQuickSlot();
            }

            var eventItemID = _inventoryData[targetSlotID].GetSlotItemID();
            var sameItemList = FindSameItem(eventItemID);
            var accItemCount = 0;
            foreach (var slotID in sameItemList)
            {
                accItemCount += _inventoryData[slotID].GetSlotItemCount();
            }
            EventManager.Notify(new UpdateInventoryEvent(this));
            EventManager.Notify(new ItemCountChangedEvent(slotItemID, accItemCount));
            EventManager.Notify(new ItemUsedEvent(slotItemID, itemCount));
        }

        public void RemoveItem(ISlot targetSlot)
        {
            var inventorySlot = _inventoryData[targetSlot.GetSlotID()];
            var slotItemID = inventorySlot.GetSlotItemID();
            inventorySlot.SetSlotItemID("");
            inventorySlot.SetSlotItemCount(0);

            var registrationSlot = FindSlotByItemID(SlotType.Quick, inventorySlot.GetSlotItemID());
            if (registrationSlot != null)
            {
                UpdateQuickSlot();
            }

            var eventItemID = _inventoryData[targetSlot.GetSlotID()].GetSlotItemID();
            var sameItemList = FindSameItem(eventItemID);
            var accItemCount = 0;
            foreach (var slotID in sameItemList)
            {
                accItemCount += _inventoryData[slotID].GetSlotItemCount();
            }

            EventManager.Notify(new UpdateInventoryEvent(this));
            EventManager.Notify(new ItemCountChangedEvent(slotItemID, accItemCount));
        }

        public void TransferItem(ISlot sourceSlot, ISlot targetSlot)
        {
            var target = _inventoryData[targetSlot.GetSlotID()];
            var source = _inventoryData[sourceSlot.GetSlotID()];

            var itemCount = target.GetSlotItemCount() + source.GetSlotItemCount();

            if (itemCount <= MaxUseItemCount)
            {
                target.SetSlotItemCount(itemCount);
                source.SetSlotItemID("");
                source.SetSlotItemCount(0);
            }
            else
            {
                var remainCount = itemCount - MaxUseItemCount;
                target.SetSlotItemCount(MaxUseItemCount);
                source.SetSlotItemCount(remainCount);
            }

            EventManager.Notify(new UpdateInventoryEvent(this));
        }

        public void SwapInventorySlot(int dropID, int targetID)
        {
            (_inventoryData[dropID], _inventoryData[targetID]) = (_inventoryData[targetID], _inventoryData[dropID]);
            EventManager.Notify(new UpdateInventoryEvent(this));
        }
    }
    #endregion

    #region 퀵슬롯 함수
    public partial class Inventory
    {
        public void UseQuickSlotItem(int slotID)
        {
            var slotData = _quickData[slotID];
            var index = Array.FindIndex(_inventoryData
                , data => data.GetSlotItemID() == slotData.GetSlotItemID());

            if (slotData.GetSlotItemID() == "") return;

            var count = slotData.GetSlotItemCount() - 1;

            switch (slotData.GetSlotItem().type)
            {
                case ItemType.Use:
                case ItemType.Mineral:
                    if (count > 0)
                    {
                        slotData.GetSlotItem().DisplayUseEffect();
                        slotData.SetSlotItemCount(count);
                    }
                    else
                    {
                        slotData.GetSlotItem().DisplayUseEffect();
                        slotData.SetSlotItemID("");
                        slotData.SetSlotItemCount(0);
                    }
                    break;
                case ItemType.Inherent:
                    slotData.GetSlotItem().DisplayUseEffect();
                    break;
            }

            DecreaseItemBySlotID(index);
        }

        public void SwapQuickSlot(int dropID, int targetID)
        {
            (_quickData[dropID], _quickData[targetID]) = (_quickData[targetID], _quickData[dropID]);
        }

        public void RegistrationQuickSlot(int targetSlotID, int referenceID)
        {
            var droppedSlot = _inventoryData[referenceID];

            for (int i = 0; i < MaxQuickSlots; i++)
            {
                if (_quickData[i].GetSlotItemID() == "") continue;

                if (_quickData[i].GetSlotItemID() == droppedSlot.GetSlotItemID())
                {
                    _quickData[i].SetSlotItemID("");
                    _quickData[i].SetSlotItemCount(0);
                }
            }

            _quickData[targetSlotID].SetSlotItemID(droppedSlot.GetSlotItemID());
            UpdateQuickSlot();

            EventManager.Notify(new UpdateInventoryEvent(this));
        }

        private void UpdateQuickSlot()
        {
            var accItemCount = 0;

            foreach (var data in _quickData)
            {
                if (data.GetSlotItemID() == "") continue;

                var itemListIndex = FindSameItem(data.GetSlotItemID());
                if (itemListIndex.Count == 0)
                {
                    data.SetSlotItemID("");
                    data.SetSlotItemCount(0);
                    return;
                }

                foreach (var index in itemListIndex)
                {
                    accItemCount += _inventoryData[index].GetSlotItemCount();
                }

                data.SetSlotItemCount(accItemCount);
                accItemCount = 0;
            }
        }
    }
    #endregion

    #region 아티팩트 함수
    public partial class Inventory
    {
        public void EquipArtifact(ISlot inventorySlot)
        {
            var artifactItemSlot = FindSlot(SlotType.Inventory, inventorySlot.GetSlotID());
            if (artifactItemSlot.GetItemType() != ItemType.Equipment) return;
            foreach (var artifactSlot in _artifactSlots)
                if (artifactSlot.GetSlotItemID() == artifactItemSlot.GetSlotItemID())
                    return;

            var emptySlot = FindSlotByItemID(SlotType.Artifact, "");
            if (emptySlot == null) return;

            emptySlot.SetSlotItemID(artifactItemSlot.GetSlotItemID());
            artifactItemSlot.SetSlotItemID("");
            artifactItemSlot.SetSlotItemCount(0);

            EventManager.Notify(new UpdateInventoryEvent(this));
            artifactChanged?.Invoke();

            // 메뉴 등록
        }

        public void EquipDraggedArtifact(ISlot inventorySlot, ISlot targetSlot)
        {
            var draggedSlot = FindSlot(SlotType.Inventory, inventorySlot.GetSlotID());
            if (draggedSlot.GetItemType() != ItemType.Equipment) return;
            foreach (var artifactSlot in _artifactSlots)
                if (artifactSlot.GetSlotItemID() == draggedSlot.GetSlotItemID())
                    artifactSlot.SetSlotItemID("");

            _artifactSlots[targetSlot.GetSlotID()].SetSlotItemID(draggedSlot.GetSlotItemID());
            draggedSlot.SetSlotItemID("");
            draggedSlot.SetSlotItemCount(0);

            EventManager.Notify(new UpdateInventoryEvent(this));
            artifactChanged?.Invoke();

            // 드래그 드롭 등록
        }

        public void UnEquipArtifact(ISlot targetSlot)
        {
            var emptySlot = FindSlotByItemID(SlotType.Inventory, "");
            var artifactItemSlot = _artifactSlots[targetSlot.GetSlotID()];

            emptySlot.SetSlotItemID(artifactItemSlot.GetSlotItemID());
            emptySlot.SetSlotItemCount(MaxInherentItemCount);

            _artifactSlots[targetSlot.GetSlotID()].SetSlotItemID("");

            EventManager.Notify(new UpdateInventoryEvent(this));
            artifactChanged?.Invoke();
        }

        public void SwapArtifact(ISlot draggedSlot, ISlot targetSlot)
        {
            if (_artifactSlots[draggedSlot.GetSlotID()].GetSlotItemID() == "") return;

            (_artifactSlots[draggedSlot.GetSlotID()], _artifactSlots[targetSlot.GetSlotID()])
                = (_artifactSlots[targetSlot.GetSlotID()], _artifactSlots[draggedSlot.GetSlotID()]);
            EventManager.Notify(new UpdateInventoryEvent(this));
            artifactChanged?.Invoke();
        }
    }
    #endregion
    // 검색
    public partial class Inventory
    {
        public bool CheckMaxInventory(string itemID, int itemCount = 1)
        {
            var list = FindSameItem(itemID);
            var acc = 0;
            foreach (var idx in list)
            {
                acc += MaxUseItemCount - _inventoryData[idx].GetSlotItemCount();
            }

            if (acc >= itemCount)
                return false;
            
            var emptyList = FindSameItem("");
                 
            if (emptyList.Count != 0)
                return false;
            
            return true;
        }
        public Slot[] GetSlots(SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.Inventory:
                    return _inventoryData;
                case SlotType.Quick:
                    return _quickData;
                case SlotType.Artifact:
                    return _artifactSlots;
                default:
                    throw new InvalidOperationException($"{nameof(slotType)}는 유효하지 않습니다.");
            }
        }

        public Slot FindSlot(SlotType slotType, int slotID)
        {
            switch (slotType)
            {
                case SlotType.Inventory:
                    return _inventoryData[slotID];
                case SlotType.Quick:
                    return _quickData[slotID];
                case SlotType.Artifact:
                    return _artifactSlots[slotID];
                default:
                    throw new InvalidOperationException($"{nameof(slotType)}는 유효하지 않습니다.");
            }
        }
        public List<int> FindSameItem(string itemID)
        {
            var slotCount = _inventoryData.Length;
            var sameIDList = new List<int>();

            for (int i = 0; i < slotCount; i++)
            {
                if (_inventoryData[i].GetSlotItemID() == itemID)
                    sameIDList.Add(i);
            }
            return sameIDList;
        }

        private Slot? FindSlotByItemID(SlotType slotType, string itemID)
        {
            if (slotType == SlotType.Inventory)
            {
                foreach (var data in _inventoryData)
                {
                    if (data.GetSlotItemID() == itemID)
                        return data;
                }
            }
            else if (slotType == SlotType.Quick)
            {
                foreach (var data in _quickData)
                {
                    if (data.GetSlotItemID() == itemID)
                        return data;
                }
            }
            else if (slotType == SlotType.Artifact)
            {
                foreach (var data in _artifactSlots)
                {
                    if (data.GetSlotItemID() == itemID)
                        return data;
                }
            }
            else
            {
                throw new InvalidOperationException($"{nameof(slotType)}는 유효하지 않습니다.");
            }

            return null;
        }
    }
}
