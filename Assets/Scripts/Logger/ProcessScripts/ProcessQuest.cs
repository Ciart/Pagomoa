using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProcessQuest
    {
        public int questId;
        public int nextQuestId;
        public string description;
        public Reward reward;
        public List<IQuestElements> elements;
        public bool accomplishment = false;

        ~ProcessQuest(){
            EventManager.RemoveListener<QuestAccomplishEvent>(QuestAccomplishment);
        }
        
        public ProcessQuest(int questId, int nextQuestId, string description, Reward reward,
            List<QuestCondition> questConditions)
        {
            this.questId = questId;
            this.nextQuestId = nextQuestId;
            this.description = description;
            this.reward = reward;
            elements = new List<IQuestElements>();

            EventManager.AddListener<QuestAccomplishEvent>(QuestAccomplishment);
            
            foreach (var condition in questConditions)
            {
                switch (condition.questType)
                {
                    case QuestType.CollectMineral :
                        var collectMineral = new CollectMineral(condition);
                        elements.Add(collectMineral);                        
                        break;
                    case QuestType.BreakBlock :
                        var breakBlock = new BreakBlock(condition);
                        elements.Add(breakBlock);                        
                        break;
                }
            }
        }

        private void QuestAccomplishment(QuestAccomplishEvent e)
        {
            foreach (var element in elements)
            {
                if (!element.complete)
                {
                    accomplishment = false;
                    EventManager.Notify(new SignalToNpc(accomplishment));
                    return ;
                }
            }
            
            accomplishment = true;
            EventManager.Notify(new SignalToNpc(accomplishment));
        }

        public MineralItem GetMineralItemEntity()
        {
            return (MineralItem)reward.targetEntity;
        }
        public Ground GetGroundEntity()
        {
            return (Ground)reward.targetEntity;
        }
    }

    public interface IQuestElements
    {
        public bool complete { get; set; }
        public bool CheckComplete();
        public string GetQuestSummary();
        public string GetValueToString();
        public string GetCompareValueToString();
    }
    
    #region IntQuestElements
    public class CollectMineral : ProcessIntQuestElements, IQuestElements
    {
        public bool complete { get; set; } = false;
        
        ~CollectMineral() {
            EventManager.RemoveListener<ItemCountEvent>(CountItem);
        }
        public CollectMineral(QuestCondition elements)
        {
            questType = elements.questType; 
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetEntity = elements.targetEntity;
            valueType = elements.value; 
            compareValue = 0;
            
            EventManager.AddListener<ItemCountEvent>(CountItem);

            var inventoryItems = InventoryDB.Instance.items;
            foreach (var inventoryItem in inventoryItems)
            {
                if (inventoryItem.item == targetEntity)
                {
                    EventManager.Notify(new ItemCountEvent(inventoryItem.item, inventoryItem.count));
                    EventManager.Notify(new QuestAccomplishEvent());
                    break;
                }
            }
        }

        

        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            return target == targetEntity;
        }

        public override void CalculationValue(IEvent e)
        {
            var inventoryItem = (ItemCountEvent)e;
            
            InitQuestValue(value, inventoryItem.itemCount);
            
            Debug.Log("mineral :" + compareValue);
        }

        private void CountItem(ItemCountEvent e)
        {
            if (!TypeValidation(e.item)) return ;

            var prevValue = compareValue;

            CalculationValue(e);
            
            if (CheckComplete()) EventManager.Notify(new QuestAccomplishEvent());
            if (e.itemCount <= prevValue) EventManager.Notify(new QuestAccomplishEvent());
        }

        public string GetQuestSummary()
        {
            return summary;
        }

        public string GetValueToString()
        {
            return value.ToString();
        }

        public string GetCompareValueToString()
        {
            return compareValue.ToString();
        }
    }
    public class BreakBlock : ProcessIntQuestElements, IQuestElements
    {
        public bool complete { get; set; } = false;
        
        ~BreakBlock() {
            EventManager.RemoveListener<GroundBrokenEvent>(OnGroundBroken);
        }
        public BreakBlock(QuestCondition elements)
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

        public override bool TypeValidation(ScriptableObject target)
        {
            return target == targetEntity;
        }

        public override void CalculationValue(IEvent e)
        {
            compareValue++;
            
            Debug.Log("Block :" + compareValue);
        }
        
        private void OnGroundBroken(GroundBrokenEvent e)
        {
            if (!TypeValidation(e.brick.ground)) return;
            CalculationValue(e);

            if (CheckComplete()) EventManager.Notify(new QuestAccomplishEvent());
        }
        
        public string GetQuestSummary()
        {
            return summary;
        }

        public string GetValueToString()
        {
            return value.ToString();
        }

        public string GetCompareValueToString()
        {
            return compareValue.ToString();
        }
    }
    #endregion
    
    #region FloatQuestElements
    
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}