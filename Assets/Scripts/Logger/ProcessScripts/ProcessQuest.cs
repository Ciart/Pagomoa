using System;
using System.Collections.Generic;
using Logger.ForEditorBaseScripts;
using UnityEngine;
using UnityEngine.Events;
using Worlds;

namespace Logger.ProcessScripts
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
            // 인벤토리 수집요소 아이템은 checkComplete 돌려보기 
        }

        public override bool CheckComplete()
        {
            var inventoryItems = InventoryDB.Instance.items;
            
            foreach (var inventory in inventoryItems)
            {
                if (inventory.item == targetEntity && inventory.count == value)
                {
                    compareValue = inventory.count;
                    return true;
                }
                if (inventory.item == targetEntity && inventory.count != value)
                {
                    compareValue = inventory.count;
                    return false;
                }
            }
            return false;
        }

        public override void CalculationValue()
        {
            if (CheckComplete() && compareValue >= value ) return;
            
            compareValue++;
        }
    }
    
    public class ConsumeMineral : ProcessIntQuestElements
    {
        public override bool CheckComplete()
        {
            throw new System.NotImplementedException();
        }

        public override void CalculationValue()
        {
            throw new System.NotImplementedException();
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

        public override void CalculationValue()
        {
            if (CheckComplete())
            {
                // todo 퀘스트 완료 되면 퀘스트 npc에게 완료 신호 보내기
                erase();
                return ;
            }
            compareValue++;
            Debug.Log(compareValue);
        }
    }
    #endregion
    
    #region FloatQuestElements
    
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}