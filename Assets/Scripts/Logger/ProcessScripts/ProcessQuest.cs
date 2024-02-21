using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
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
            
            EventManager.AddListener<GroundBrokenEvent>(OnGroundBroken);
        }

        public override bool CheckComplete()
        {
            return compareValue >= value;
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            Debug.Log(targetEntity);
            Debug.Log((Mineral)target);
            return targetEntity == target;
        }
        
        public bool TypeValidationA(Mineral target)
        {
            Debug.Log(targetEntity);
            Debug.Log(target);
            return targetEntity == target;
        }

        private void OnGroundBroken(GroundBrokenEvent e)
        {
            if ( !TypeValidationA(e.brick.mineral) ) return ;  
            CalculationValue();
        }
        
        public override void CalculationValue()
        {
            compareValue = SearchItemCount();
        
            Debug.Log("min :" + compareValue);
            
            CheckComplete();
        }

        private int SearchItemCount()
        {
            foreach (var inventory in _inventoryDB.items)
            {
                if (inventory.item is null) return compareValue;
                return inventory.count;
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
            return compareValue >= value;
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            // e.brick : Brick
            // targetEntity : Ground
            throw new NotImplementedException();
        }


        private void OnGroundBroken(GroundBrokenEvent e)
        {
            CalculationValue();
        }
        
        public override void CalculationValue()
        {
            if (CheckComplete())
            {
                // todo 퀘스트 완료 되면 퀘스트 npc에게 완료 신호 보내기
                
                return ;
            }
            
            compareValue++; 
            
            Debug.Log("Block :" + compareValue);

            CheckComplete();
        }
    }
    #endregion
    
    #region FloatQuestElements
    
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}