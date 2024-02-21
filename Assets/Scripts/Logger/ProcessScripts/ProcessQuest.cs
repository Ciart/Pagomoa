using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Logger.ForEditorBaseScripts;
using Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProcessQuest
    {
        public int questId;
        public int nextQuestId;
        public string description;
        public Reward reward;
        public List<ProcessQuestElements> elements;
        public bool accomplishment = false;

        public ProcessQuest(int questId, int nextQuestId, string description, Reward reward,
            List<QuestCondition> questConditions)
        {
            this.questId = questId;
            this.nextQuestId = nextQuestId;
            this.description = description;
            this.reward = reward;
            elements = new List<ProcessQuestElements>();

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

        public void QuestAcomplishment()
        {
            foreach (var element in elements)
            {
                if (!element.complete) break;
            }

            accomplishment = true;
        }
    }
    
    #region IntQuestElements
    public class CollectMineral : ProcessIntQuestElements
    {
        private readonly InventoryDB _inventoryDB = InventoryDB.Instance;
        public CollectMineral(QuestCondition elements)
        {
            questType = elements.questType; 
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetEntity = elements.targetEntity;
            valueType = elements.value; 
            compareValue = SearchItemCount();
            
            EventManager.AddListener<CollectItemEvent>(CollectItem);
            EventManager.AddListener<ConsumeItemEvent>(ConsumeItem);
        }

        public override bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public override bool TypeValidation(ScriptableObject target)
        { 
            throw new NotImplementedException();
        }

        private void CollectItem(CollectItemEvent e)
        {
            // TypeValidation
            CalculationValue();
            
            Debug.Log("mineral :" + compareValue);
        }

        private void ConsumeItem(ConsumeItemEvent e)
        {
            // TypeValidation
            CalculationValue();
            
            if (e.item.item == null) compareValue = 0;
            
            Debug.Log("mineral :" + compareValue);
        }
        
        public override void CalculationValue()
        {
            compareValue = SearchItemCount();
            
            CheckComplete();
        }

        private int SearchItemCount()
        {
            foreach (var inventory in _inventoryDB.items)
            {
                if (inventory.item == targetEntity)
                {
                    return inventory.count;
                }
            }
            
            return compareValue;
        }
    }
    public class BreakBlock : ProcessIntQuestElements
    {
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

        public override bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            
            throw new NotImplementedException();
        }

        private void OnGroundBroken(GroundBrokenEvent e)
        {
            CalculationValue();
        }
        
        public override void CalculationValue()
        {
            compareValue++; 
            
            Debug.Log("Block :" + compareValue);

            complete = CheckComplete();
        }
    }
    #endregion
    
    #region FloatQuestElements
    
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}