using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Systems.Save;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public enum QuestState
    {
        UnRegister,
        InProgress,
        Completed,
        Finish
    }
    public class Quest
    {
        public string id { get; private set; }
        public string description { get; private set; }
        public string title { get; private set; }
        public Reward reward { get; private set; }
        public readonly List<IQuestElements> conditions;
        public Sprite? npcSprite { get; private set; }
        public QuestState state;
        public float progress;

        public Quest(QuestData questData)
        {
            id = questData.id;
            description = questData.description;
            title = questData.title;
            reward = questData.reward;
            conditions = new List<IQuestElements>();
            state = QuestState.InProgress;
            if (questData.owner != null) npcSprite = ResourceSystem.instance.GetEntity(questData.owner).portrait;

            foreach (var condition in questData.questList)
            {
                switch (condition.questType)
                {
                    case QuestType.CollectItem:
                        var collectItem = new CollectItem(condition);
                        collectItem.questFinished = AllElementsFinish;
                        conditions.Add(collectItem);
                        break;
                    case QuestType.UseItem:
                        var useItem = new UseItem(condition);
                        useItem.questFinished = AllElementsFinish;
                        conditions.Add(useItem);
                        break;
                    case QuestType.BreakBlock:
                        var breakBlock = new BreakBlock(condition);
                        breakBlock.questFinished = AllElementsFinish;
                        conditions.Add(breakBlock);
                        break;
                    case QuestType.HasItem:
                        var hasItem = new HasItem(condition);
                        hasItem.questFinished = AllElementsFinish;
                        conditions.Add(hasItem);
                        hasItem.CheckComplete();
                        hasItem.questFinished.Invoke();
                        break;
                    case QuestType.SellItem:
                        var sellItem = new SellItem(condition);
                        sellItem.questFinished = AllElementsFinish;
                        conditions.Add(sellItem);
                        break;
                }
            }
        }

        private void AllElementsFinish()
        {
            progress = 0.0f;

            var allFinish = true;

            foreach (var condition in conditions)
            {
                progress += condition.GetProgress() / conditions.Count;

                if (condition.CheckComplete() == false)
                {
                    allFinish = false;
                }
            }

            EventManager.Notify(new QuestUpdated(this));
            EventManager.Notify(new QuestListUpdated(Game.Instance.Quest.quests));

            if (allFinish)
            {
                progress = 1.0f;
                state = QuestState.Completed;
            }
            else
            {
                state = QuestState.InProgress;
            }

            EventManager.Notify(new QuestCompleted(this));
        }

        public void Finish()
        {
            EventManager.Notify(new QuestUpdated(this));
            EventManager.Notify(new QuestListUpdated(Game.Instance.Quest.quests));

            foreach (var element in conditions)
            {
                element.Finish();
            }
        }
    }

    public interface IQuestElements
    {
        public QuestType questType { get; }
        public bool CheckComplete();
        public float GetProgress();
        public string GetTargetID();
        public string GetQuestSummary();
        public string GetValueToString();
        public string GetCompareValueToString();
        public int GetCompareValue();
        public void SetCompareValue(int value);
        public void Finish();
    }

    #region CollectItem : 아이템 수집 (수집한 아이템은 퀘스트를 완료해도 사라지지 않음)
    public class CollectItem : QuestCondition, IQuestElements
    {
        private int _prevValue;
        public CollectItem(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetId = elements.targetID;
            valueType = elements.value;
            compareValue = 0;
            _prevValue = 0;

            EventManager.AddListener<ItemCountChangedEvent>(CountItem);
            EventManager.AddListener<LoadInventoryEvent>(OnLoadInventory);
        }

        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public float GetProgress()
        {
            var fCompareValue = (float)compareValue;
            var fValue = (float)value;

            progress = fCompareValue / fValue;

            return progress;
        }

        public string GetTargetID()
        {
            return targetId;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public override void CalculationValue(IEvent e)
        {
            var inventoryItem = (ItemCountChangedEvent)e;

            if (_prevValue > inventoryItem.count)
            {
                _prevValue = inventoryItem.count;
                return;
            }

            var collectValue = inventoryItem.count - _prevValue;

            if (compareValue >= collectValue)
            {
                compareValue = value;
            }
            else
            {
                compareValue = collectValue;
            }
        }

        private void CountItem(ItemCountChangedEvent e)
        {
            if (!TypeValidation(e.itemID)) return;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        private void OnLoadInventory(LoadInventoryEvent e)
        {
            var inventory = e.inventory;
            var sameSlots = inventory.FindSameItem(targetId);
            var inventoryList = inventory.GetSlots(SlotType.Inventory);

            foreach (var slot in sameSlots)
            {
                _prevValue += inventoryList[slot].GetSlotItemCount();
            }
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
        public void Finish() { EventManager.RemoveListener<ItemCountChangedEvent>(CountItem); }

        public int GetCompareValue()
        {
            return compareValue;
        }
        public void SetCompareValue(int value)
        {
            compareValue = value;
        }
    }
    #endregion
    #region UseItem 소모성, 액티브 아이템 사용 
    public class UseItem : QuestCondition, IQuestElements
    {
        public UseItem(QuestConditionData conditions)
        {
            questType = conditions.questType;
            summary = conditions.summary;
            value = int.Parse(conditions.value);
            targetId = conditions.targetID;
            valueType = conditions.value;
            compareValue = 0;

            EventManager.AddListener<ItemUsedEvent>(HasUsingItem);
        }

        private void HasUsingItem(ItemUsedEvent e)
        {
            if (!TypeValidation(e.itemID)) return;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public override void CalculationValue(IEvent e)
        {
            if (compareValue == value) return;
            var usedItemEvent = (ItemUsedEvent)e;
            
            Debug.Log($"{compareValue} & {usedItemEvent.count} & {value}");
            compareValue += usedItemEvent.count;

            if (compareValue > value) compareValue = value;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public float GetProgress()
        {
            var fCompareValue = (float)compareValue;
            var fValue = (float)value;

            progress = fCompareValue / fValue;

            return progress;
        }

        public string GetTargetID()
        {
            return targetId;
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
        public void Finish() { EventManager.RemoveListener<ItemUsedEvent>(HasUsingItem); }

        public int GetCompareValue()
        {
            return compareValue;
        }
        public void SetCompareValue(int value)
        {
            compareValue = value;
        }
    }
    #endregion
    #region BreakBlock      
    public class BreakBlock : QuestCondition, IQuestElements
    {
        public BreakBlock(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            targetId = elements.targetID;
            valueType = elements.value;
            value = int.Parse(elements.value);
            compareValue = 0;

            EventManager.AddListener<GroundBrokenEvent>(OnGroundBroken);
        }

        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public float GetProgress()
        {
            var fCompareValue = (float)compareValue;
            var fValue = (float)value;

            progress = fCompareValue / fValue;

            return progress;
        }

        public string GetTargetID()
        {
            return targetId;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public override void CalculationValue(IEvent e)
        {
            if (compareValue == value) return;
            compareValue++;
        }

        private void OnGroundBroken(GroundBrokenEvent e)
        {
            if (!TypeValidation(e.brick.groundId)) return;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
        public void Finish() { EventManager.RemoveListener<GroundBrokenEvent>(OnGroundBroken); }

        public int GetCompareValue()
        {
            return compareValue;
        }
        public void SetCompareValue(int value)
        {
            compareValue = value;
        }
    }
    #endregion

    #region HasItem 
    public class HasItem : QuestCondition, IQuestElements
    {
        public HasItem(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetId = elements.targetID;
            valueType = elements.value;
            compareValue = 0;

            EventManager.AddListener<ItemCountChangedEvent>(CountItem);
            EventManager.AddListener<LoadInventoryEvent>(CheckItemFromInventory);
        }

        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public float GetProgress()
        {
            var fCompareValue = (float)compareValue;
            var fValue = (float)value;

            progress = fCompareValue / fValue;

            return progress;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public override void CalculationValue(IEvent e)
        {
            var inventoryItem = (ItemCountChangedEvent)e;

            compareValue = inventoryItem.count;
        }

        private void CountItem(ItemCountChangedEvent e)
        {
            if (!TypeValidation(e.itemID)) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public string GetTargetID() { return targetId; }
        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
        public void Finish()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(CountItem);
            EventManager.RemoveListener<LoadInventoryEvent>(CheckItemFromInventory);

            var itemList = Game.Instance.player.inventory.FindSameItem(GetTargetID());

            foreach (var index in itemList)
            {
                var slot = Game.Instance.player.inventory.FindSlot(SlotType.Inventory, index);
                if (slot.GetSlotItemCount() >= value)
                {
                    Game.Instance.player.inventory.DecreaseItemBySlotID(index, value);
                    break;
                }
            }
        }

        private void CheckItemFromInventory(LoadInventoryEvent e)
        {
            var items = e.inventory.FindSameItem(targetId);
            foreach (var itemCount in items)
            {
                compareValue += itemCount;
            }
            
            CheckComplete();
            questFinished.Invoke();
        }

        public int GetCompareValue()
        {
            return compareValue;
        }
        public void SetCompareValue(int value)
        {
            compareValue = value;
        }
    }
    #endregion

    #region SellItem

    public class SellItem : QuestCondition, IQuestElements
    {
        public SellItem(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetId = elements.targetID;
            valueType = elements.value;
            compareValue = 0;

            EventManager.AddListener<ItemSellEvent>(SellingItem);
        }

        private void SellingItem(ItemSellEvent e)
        {
            if (!TypeValidation(e.itemID)) return;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public override void CalculationValue(IEvent e)
        {
            var sellEvent = (ItemSellEvent)e;

            compareValue += sellEvent.count;
            if (compareValue >= value) compareValue = value;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public bool CheckComplete()
        {
            return compareValue >= value;
        }

        public float GetProgress()
        {
            var fCompareValue = (float)compareValue;
            var fValue = (float)value;

            progress = fCompareValue / fValue;

            return progress;
        }

        public string GetTargetID() { return targetId; }
        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
        public void Finish() { EventManager.RemoveListener<ItemSellEvent>(SellingItem); }

        public int GetCompareValue()
        {
            return compareValue;
        }
        public void SetCompareValue(int value)
        {
            compareValue = value;
        }
    }
    #endregion
}
