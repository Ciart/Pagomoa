using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


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

        private void Awake()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
            InitSlots();
        }

        private void Destroy()
        {
            EventManager.RemoveListener<AddReward>(AddReward);
            EventManager.RemoveListener<AddGold>(ChangeGold);
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
        private void AddReward(AddReward e) { AddInventory(e.item.id, e.itemCount); }
        private void AddGold(int addGold) { gold += addGold; }
    }

    #region 인벤토리 함수
    public partial class Inventory
    {
        public void AddInventory(string itemID, int itemCount = 1)
        {
            var hasItemSlot = Array.FindIndex(_inventoryData,
                (data) => data.GetSlotItemID() == itemID);

            if (hasItemSlot > -1)
            {
                var accCount = itemCount + _inventoryData[hasItemSlot].GetSlotItemCount();

                if (accCount <= MaxUseItemCount) // 슬롯에 할당 가능한 아이템 상한  
                {
                    _inventoryData[hasItemSlot].SetSlotItemID(itemID);
                    _inventoryData[hasItemSlot].SetSlotItemCount(accCount);
                }
                else // 2000 
                {
                    accCount -= MaxUseItemCount;
                    _inventoryData[hasItemSlot].SetSlotItemCount(MaxUseItemCount);

                    var divVal = (int)(accCount / MaxUseItemCount);
                    var remainVal = accCount % MaxUseItemCount;

                    if (divVal > 0)
                    {
                        for (int divValIndex = 0; divValIndex < divVal; divValIndex++)
                        {
                            var emptySlot = FindSlotByItemID(SlotType.Inventory, "");

                            if (emptySlot != null)
                            {
                                emptySlot.SetSlotItemID(itemID);
                                emptySlot.SetSlotItemCount(MaxUseItemCount);
                            }
                        }
                    }
                    if (remainVal != 0)
                    {
                        var emptySlot = FindSlotByItemID(SlotType.Inventory, "");

                        if (emptySlot != null)
                        {
                            emptySlot.SetSlotItemID(itemID);
                            emptySlot.SetSlotItemCount(remainVal);
                        }
                    }
                }
                // TODO : 사용 아이템 이외에는 복수 장착이 있는가?
            }
            else
            {
                if (itemCount <= MaxUseItemCount)
                {
                    var hasEmptySlot = Array.FindIndex(_inventoryData,
                        (data) => data.GetSlotItemID() == "");

                    _inventoryData[hasEmptySlot].SetSlotItemID(itemID);
                    _inventoryData[hasEmptySlot].SetSlotItemCount(itemCount);
                }
                else
                {
                    var divVal = (int)(itemCount / MaxUseItemCount);
                    var remainVal = itemCount % MaxUseItemCount;

                    if (divVal > 0)
                    {
                        for (int divValIndex = 0; divValIndex < divVal; divValIndex++)
                        {
                            var emptySlot = FindSlotByItemID(SlotType.Inventory, "");

                            if (emptySlot != null)
                            {
                                emptySlot.SetSlotItemID(itemID);
                                emptySlot.SetSlotItemCount(MaxUseItemCount);
                            }
                        }
                    }
                    if (remainVal != 0)
                    {
                        var emptySlot = FindSlotByItemID(SlotType.Inventory, "");

                        if (emptySlot != null)
                        {
                            emptySlot.SetSlotItemID(itemID);
                            emptySlot.SetSlotItemCount(remainVal);
                        }
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
            EventManager.Notify(new UpdateInventory());
            EventManager.Notify(new ItemCountChangedEvent(itemID, accItemCount));
        }

        public void SellItem(ISlot targetSlot)
        {
            gold += _inventoryData[targetSlot.GetSlotID()].GetSlotItem().price;
            DecreaseItemBySlotID(targetSlot.GetSlotID());

            UIManager.instance.UpdateGoldUI();
        }

        public void DecreaseItemBySlotID(int targetSlotID, int itemCount = 1)
        {
            var inventorySlot = _inventoryData[targetSlotID];
            var count = inventorySlot.GetSlotItemCount() - itemCount;

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
            EventManager.Notify(new UpdateInventory());
            EventManager.Notify(new ItemCountChangedEvent(eventItemID, accItemCount));
        }

        public void RemoveItem(ISlot targetSlot)
        {
            var inventorySlot = _inventoryData[targetSlot.GetSlotID()];
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

            EventManager.Notify(new UpdateInventory());
            EventManager.Notify(new ItemCountChangedEvent(eventItemID, accItemCount));
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
            
            EventManager.Notify(new UpdateInventory());
        }
        
        public void SwapInventorySlot(int dropID, int targetID)
        {
            (_inventoryData[dropID], _inventoryData[targetID]) = (_inventoryData[targetID], _inventoryData[dropID]);
            EventManager.Notify(new UpdateInventory());
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

            EventManager.Notify(new UpdateInventory());
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
                
            EventManager.Notify(new UpdateInventory());

            // 메뉴 등록
            // TODO : 플레이어 스텟 적용
            // TODO : GetSlots(SlotType.Artifact);로 가져가서 아이템 확인후 스텟 적용 하면 될듯 
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
            
            EventManager.Notify(new UpdateInventory());

            // 드래그 드롭 등록
            // TODO : 플레이어 스텟 적용
        }

        public void UnEquipArtifact(ISlot targetSlot)
        {
            var emptySlot = FindSlotByItemID(SlotType.Inventory, "");
            var artifactItemSlot = _artifactSlots[targetSlot.GetSlotID()];
            
            emptySlot.SetSlotItemID(artifactItemSlot.GetSlotItemID());
            emptySlot.SetSlotItemCount(MaxInherentItemCount);
            
            _artifactSlots[targetSlot.GetSlotID()].SetSlotItemID("");
            
            EventManager.Notify(new UpdateInventory());
            // TODO : 플레이어 스텟 적용
        }

        public void SwapArtifact(ISlot draggedSlot, ISlot targetSlot)
        {
            if (_artifactSlots[draggedSlot.GetSlotID()].GetSlotItemID() == "") return;

            (_artifactSlots[draggedSlot.GetSlotID()], _artifactSlots[targetSlot.GetSlotID()])
                = (_artifactSlots[targetSlot.GetSlotID()], _artifactSlots[draggedSlot.GetSlotID()]);
            EventManager.Notify(new UpdateInventory());
        }
    }
    #endregion
    // 검색
    public partial class Inventory
    {
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
/*
 *
             아이템 있는지 확인
             if 아이템 있음 itemtype
                if 더했을때 Max 수치 보다 작으면
                    그냥 더함 
                else if 더했을때 Max 수치 보다 크면
                    1. 아이템 있는 슬롯 Max 까지 채우기
                    2. 남은 아이템들 (int)(itemCount / Max) 만큼 빈 슬롯 찾기 Max할당
                    3. (itemCount % Max)가 있다면 빈 슬롯 찾고 나머지 할당
                if 퀵슬롯 찾아서 refID있으면 
                    1. refID 추가 & 수량 조정 
             if 아이템 없음 itemtype
                if 더했을때 Max 수치 보다 작으면
                    빈 슬롯 찾고 아이템 할당, 아이템 수량 체크
                else if 더했을때 Max 수치 보다 크면
                    1. 남은 아이템들 (int)(itemCount / Max) 만큼 빈 슬롯 찾기 Max할당
                    2. (itemCount % Max)가 있다면 빈 슬롯 찾고 나머지 할당
                if 퀵슬롯 찾아서 refID있으면 
                    1. refID 추가 & 수량 조정 
 */
