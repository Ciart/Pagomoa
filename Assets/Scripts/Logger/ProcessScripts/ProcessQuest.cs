using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProcessQuest : IDisposable
    {
        public string id;
        public string description;
        public string title;
        public Reward reward;
        public List<IQuestElements> elements;
        public bool accomplishment = false;

        public ProcessQuest(Quest quest)
        {
            id = quest.id;
            description = quest.description;
            title = quest.title;
            reward = quest.reward;
            elements = new List<IQuestElements>();

            EventManager.AddListener<QuestAccomplishEvent>(QuestAccomplishment);

            foreach (var condition in quest.questList)
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
            
            EventManager.Notify(new QuestAccomplishEvent());
        }

        private void QuestAccomplishment(QuestAccomplishEvent e)
        {
            foreach (var element in elements)
            {
                if (element.complete == false)
                {
                    accomplishment = false;
                    EventManager.Notify(new SignalToNpc(id, accomplishment));
                    return ;
                }
            }
            
            accomplishment = true;
            EventManager.Notify(new SignalToNpc(id, accomplishment));
        }

        public void Dispose()
        {
            EventManager.RemoveListener<QuestAccomplishEvent>(QuestAccomplishment);

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
        public string GetQuestSummary();
        public string GetValueToString();
        public string GetCompareValueToString();
    }
    
    #region IntQuestElements
    
    public class CollectMineral : ProcessIntQuestElements, IQuestElements
    {
        public bool complete { get; set; } = false;

        public void Dispose()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(CountItem);
        }

        public CollectMineral(QuestCondition elements)
        {
            questType = elements.questType; 
            summary = elements.summary;
            value = int.Parse(elements.value);
            targetEntity = elements.targetEntity;
            valueType = elements.value; 
            compareValue = 0;
            
            EventManager.AddListener<ItemCountChangedEvent>(CountItem);

            var inventoryItems = GameManager.player.inventoryDB.items;
            foreach (var inventoryItem in inventoryItems)
            {
                if (inventoryItem.item == targetEntity)
                {
                    compareValue = inventoryItem.count;
                    if (CheckComplete()) EventManager.Notify(new QuestAccomplishEvent());
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
            var inventoryItem = (ItemCountChangedEvent)e;
            
            InitQuestValue(value, inventoryItem.count);
            
            Debug.Log("mineral :" + compareValue);
        }

        private void CountItem(ItemCountChangedEvent e)
        {
            if (!TypeValidation(e.item)) return ;

            var prevValue = compareValue;

            CalculationValue(e);
            
            if (CheckComplete()) EventManager.Notify(new QuestAccomplishEvent());
            if (e.count <= prevValue) EventManager.Notify(new QuestAccomplishEvent());
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
    }
    // This quest counts blocks when player dig blocks that same Type.      
    public class BreakBlock : ProcessIntQuestElements, IQuestElements
    {
        public bool complete { get; set; } = false;
        
        public void Dispose()
        {
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
        
        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString(); }
        public string GetCompareValueToString() { return compareValue.ToString(); }
    }
    #endregion
    
    #region FloatQuestElements
    
    
    public class PlayerMoveDistance : ProcessFloatQuestElements, IQuestElements
    {
        public bool complete { get; set; }
        private Vector3 _previousPos = Vector3.zero;

        ~PlayerMoveDistance() {
            EventManager.RemoveListener<PlayerMove>(MoveDistance);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public PlayerMoveDistance(QuestCondition elements)
        {
            questType = elements.questType;
            summary = elements.summary;
            targetEntity = elements.targetEntity;
            valueType = elements.value;
            value = float.Parse(elements.value);
            compareValue = 0.0f;

            EventManager.AddListener<PlayerMove>(MoveDistance);
        }
        
        public override bool TypeValidation(ScriptableObject target)
        {
            return true;
        }

        public override void CalculationValue(IEvent e)
        {
            var playerMove = (PlayerMove)e;
            var distance = Vector3.Distance(_previousPos, playerMove.playerPos);

            compareValue += distance;
        }

        private void MoveDistance(PlayerMove e)
        {
            if (_previousPos == Vector3.zero) _previousPos = e.playerPos;

            CalculationValue(e);
            
            _previousPos = e.playerPos;
            
            if (CheckComplete()) EventManager.Notify(new QuestAccomplishEvent());
        }
        
        public bool CheckComplete()
        {
            complete = compareValue >= value;
            return complete;
        }

        public string GetQuestSummary() { return summary; }
        public string GetValueToString() { return value.ToString("F1"); }
        public string GetCompareValueToString() { return compareValue.ToString("F1"); }
    }
    
    // How deeply player go down from y : 0. This quest calculate player best deep transform of y. 
    public class PlayerMoveDeeply : ProcessFloatQuestElements, IQuestElements
    {
        public bool complete { get; set; }
        ~PlayerMoveDeeply()
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override bool TypeValidation(ScriptableObject target)
        {
            throw new NotImplementedException();
        }

        public override void CalculationValue(IEvent e)
        {
            throw new NotImplementedException();
        }

        public bool CheckComplete()
        {
            throw new NotImplementedException();
        }

        public string GetQuestSummary()
        {
            throw new NotImplementedException();
        }

        public string GetValueToString()
        {
            throw new NotImplementedException();
        }

        public string GetCompareValueToString()
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    
    #region BoolQuestElements
    
    #endregion
}