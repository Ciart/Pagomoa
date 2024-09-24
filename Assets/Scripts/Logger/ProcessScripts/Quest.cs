using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class Quest : IDisposable
    {
        public string id;
        public string description;
        public string title;
        public Reward reward;
        public List<IQuestElements> elements;
        public Sprite mNpcSprite;
        public bool accomplished;
        
        public float progress;
        
        public Quest(QuestData questData, Sprite npcSprite)
        {
            id = questData.id;
            description = questData.description;
            title = questData.title;
            reward = questData.reward;
            elements = new List<IQuestElements>();
            mNpcSprite = npcSprite;
            
            foreach (var condition in questData.questList)
            {
                switch (condition.questType)
                {
                    case QuestType.CollectItem :
                        var collectMineral = new CollectItem(condition);
                        collectMineral.questFinished = AllElementsFinish;
                        elements.Add(collectMineral);                        
                        break;
                    case QuestType.BreakBlock :
                        var breakBlock = new BreakBlock(condition);
                        breakBlock.questFinished = AllElementsFinish;
                        elements.Add(breakBlock);                        
                        break;
                }
            }
        }

        private void AllElementsFinish()
        {
            progress = 0.0f;

            var allFinish = true;
            
            foreach (var element in elements)
            {
                progress += element.GetProgress() / elements.Count;
                
                if (element.complete == false)
                {
                    Debug.Log(element.complete);
                    accomplished = false;
                    allFinish = false;
                }
            }
            
            if (allFinish) progress = 1.0f;
            
            EventManager.Notify(new QuestUpdated(this));
            
            if (allFinish == false) return;
            
            accomplished = true;
            EventManager.Notify(new QuestCompleted(this));
        }

        public void Dispose()
        {
            foreach (var element in elements)
            {
                element.Dispose();
            }
        }
    }

    public interface IQuestElements : IDisposable
    {
        public QuestType questType { get; set; }
        public bool complete { get; set; }
        public bool CheckComplete();
        public float GetProgress();
        public string GetQuestSummary();
        public string GetValueToString();
        public string GetCompareValueToString();
    }
    
    #region IntQuestElements
    
    public class CollectItem : QuestCondition, IQuestElements
    {
        public bool complete { get; set; } = false;
        private int _prevValue;
        
        public void Dispose()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(CountItem);
        }

        public CollectItem(QuestConditionData elements)
        {
            questType = elements.questType; 
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetEntity = elements.targetEntity;
            valueType = elements.value; 
            compareValue = 0;
            _prevValue = 0;
            
            EventManager.AddListener<ItemCountChangedEvent>(CountItem);

            var inventoryItems = GameManager.player.inventory.items;
            foreach (var inventoryItem in inventoryItems)
            {
                if (inventoryItem.item == targetEntity)
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

        public override bool TypeValidation(ScriptableObject target)
        {
            return target == targetEntity;
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
            if (!TypeValidation(e.item)) return ;
            if (CheckComplete()) return;

            CalculationValue(e);

            CheckComplete();
            questFinished.Invoke();
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
    }
    // This quest counts blocks when player dig blocks that same Type.      
    public class BreakBlock : QuestCondition, IQuestElements
    {
        public bool complete { get; set; } = false;
        
        public void Dispose()
        {
            EventManager.RemoveListener<GroundBrokenEvent>(OnGroundBroken);
        }

        public BreakBlock(QuestConditionData elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            targetEntity = elements.targetEntity;
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

        public override bool TypeValidation(ScriptableObject target)
        {
            return target == targetEntity;
        }

        public override void CalculationValue(IEvent e)
        {
            if (compareValue == value) return ;
            compareValue++;
            
            Debug.Log("Block :" + compareValue);
        }
        
        private void OnGroundBroken(GroundBrokenEvent e)
        {
            if (!TypeValidation(e.brick.ground)) return;
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