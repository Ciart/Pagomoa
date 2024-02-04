using System;
using System.Collections.Generic;
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
                    case QuestType.ConsumeMineral :
                        var consumeMineral = new CollectMineral(condition);
                        elements.Add(consumeMineral);                        
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
        public CollectMineral(QuestCondition elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetEntity = elements.targetEntity;
            valueType = elements.value;
            compareValue = 0;

            var inventoryDB = InventoryDB.Instance;
            //inventoryDB.questEvent.AddListener(CalculationValue);
            
            foreach (var inventory in inventoryDB.items)
            {
                if (inventory.item is null || !TypeValidation(inventory.item)) return;
                compareValue = inventory.count > value ? value : inventory.count % value;
                Debug.Log(compareValue);
            }
            // todo 퀘스트 완료 전까지 개수 변동 소모가 일어나면 값 변경을 해야함 
        }

        public override bool CheckComplete()
        {
            return compareValue == value;
        }

        public sealed override bool TypeValidation(ScriptableObject target)
        {
            return targetEntity.name == target.name;
        }

        public void CalculationValue(int itemCount, Item target)
        {
            if ( !TypeValidation(target) ) return; 

            if ( CheckComplete() ) return ;
        
            compareValue = itemCount;
        
            Debug.Log("min :" + compareValue);
        }
    }
    
    public class ConsumeMineral : ProcessIntQuestElements
    {
        public override bool CheckComplete()
        {
            throw new System.NotImplementedException();
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            throw new NotImplementedException();
        }

        public override void CalculationValue()
        {
            throw new NotImplementedException();
        }
        
        public void CalculationValue(int a)
        {
            throw new NotImplementedException();
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
            
            WorldManager.instance.WorldQuestEvent.AddListener(CalculationValue);
        }
        
        public void erase()
        {
            WorldManager.instance.WorldQuestEvent.RemoveListener(CalculationValue);
        }

        public override bool CheckComplete()
        {
            return compareValue == value;
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            throw new NotImplementedException();
        }

        public override void CalculationValue()
        {
            // todo targetEntity 유효성 검사 필요 
            if (CheckComplete())
            {
                // todo 퀘스트 완료 되면 퀘스트 npc에게 완료 신호 보내기
                erase();
                return ;
            }
            
            compareValue++; 
            
            Debug.Log("Block :" + compareValue);
        }
    }
    #endregion
    
    #region FloatQuestElements
    
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}