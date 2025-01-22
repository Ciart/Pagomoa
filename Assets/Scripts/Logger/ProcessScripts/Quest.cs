using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
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
    public class Quest : IDisposable
    {
        public string id;
        public string description;
        public string title;
        public Reward reward;
        public List<IQuestElements> conditions;
        public Sprite npcSprite;
        public QuestState state;
        public float progress;
        
        public Quest(QuestData questData, Sprite npcSprite)
        {
            id = questData.id;
            description = questData.description;
            title = questData.title;
            reward = questData.reward;
            conditions = new List<IQuestElements>();
            state = QuestState.InProgress;
            this.npcSprite = npcSprite;
            
            foreach (var condition in questData.questList)
            {
                switch (condition.questType)
                {
                    case QuestType.CollectItem :
                        var collectItem = new CollectItem(condition);
                        collectItem.questFinished = AllElementsFinish;
                        conditions.Add(collectItem);                        
                        break;
                    case QuestType.UseItem :
                        var useItem = new UseItem(condition);
                        useItem.questFinished = AllElementsFinish;
                        conditions.Add(useItem);                        
                        break;
                    case QuestType.BreakBlock :
                        var breakBlock = new BreakBlock(condition);
                        breakBlock.questFinished = AllElementsFinish;
                        conditions.Add(breakBlock);                        
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
                    Debug.Log(condition.CheckComplete());
                    allFinish = false;
                }
            }
            
            if (allFinish) progress = 1.0f;
            
            EventSystem.Notify(new QuestUpdated(this));
            
            if (allFinish == false) return;
            
            state = QuestState.Completed;
            EventSystem.Notify(new QuestCompleted(this));
        }

        public void Dispose()
        {
            foreach (var element in conditions)
            {
                element.Dispose();
            }
        }
    }

    public interface IQuestElements : IDisposable
    {
        public QuestType questType { get; set; }
        public bool CheckComplete();
        public float GetProgress();
        public string GetQuestSummary();
        public string GetValueToString();
        public string GetCompareValueToString();
    }
    
    #region IntQuestElements
    
    public class CollectItem : QuestCondition, IQuestElements
    {
        private int _prevValue;
        
        public void Dispose()
        {
            EventSystem.RemoveListener<ItemCountChangedEvent>(CountItem);
        }

        public CollectItem(QuestConditionData elements)
        {
            questType = elements.questType; 
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetId = elements.targetId;
            valueType = elements.value; 
            compareValue = 0;
            _prevValue = 0;
            
            EventSystem.AddListener<ItemCountChangedEvent>(CountItem);

            var inventoryItems = Game.instance.player.inventory.items;
            foreach (var inventoryItem in inventoryItems)
            {
                if (inventoryItem.item.id == targetId)
                {
                    _prevValue = inventoryItem.count;
                    break;
                }
            }
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

            if (_prevValue > inventoryItem.count)
            {
                _prevValue = inventoryItem.count;
                return;
            }

            var collectValue = inventoryItem.count - _prevValue;
            Debug.Log("collectValue :" + collectValue);

            if (compareValue >= collectValue)
            {
                compareValue = value;
            }
            else
            {
                compareValue = collectValue;
            }
            
            Debug.Log("mineral :" + compareValue);
        }

        private void CountItem(ItemCountChangedEvent e)
        {
            // if (!TypeValidation(e.item)) return ;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
    }

    public class UseItem : QuestCondition, IQuestElements
    {
        public UseItem(QuestConditionData conditions)
        {
            questType = conditions.questType; 
            summary = conditions.summary;
            value = int.Parse(conditions.value);
            targetId = conditions.targetId;
            valueType = conditions.value; 
            compareValue = 0;
            
            EventSystem.AddListener<ItemUsedEvent>(HasUsingItem);
        }

        public void HasUsingItem(ItemUsedEvent e)
        {
            if (!TypeValidation(e.item.id)) return;
            if (CheckComplete()) return;
            
            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }
        
        public override void CalculationValue(IEvent e)
        {
            if (compareValue == value) return ;
            var usedItemEvent = (ItemUsedEvent)e;
            
            compareValue += usedItemEvent.count;
            if (compareValue > value) compareValue = value;
        }

        public override bool TypeValidation(string target)
        {
            return target == targetId;
        }

        public void Dispose()
        {
            EventSystem.RemoveListener<ItemUsedEvent>(HasUsingItem);
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

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
    }
    
    
    // This quest counts blocks when player dig blocks that same Type.      
    public class BreakBlock : QuestCondition, IQuestElements
    { 
        public void Dispose()
        {
            EventSystem.RemoveListener<GroundBrokenEvent>(OnGroundBroken);
        }

        public BreakBlock(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            targetId = elements.targetId;
            valueType = elements.value;
            value = int.Parse(elements.value);
            compareValue = 0;
            
            EventSystem.AddListener<GroundBrokenEvent>(OnGroundBroken);
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
            if (compareValue == value) return ;
            compareValue++;
            
            Debug.Log("Block :" + compareValue);
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
    }
    #endregion
}
