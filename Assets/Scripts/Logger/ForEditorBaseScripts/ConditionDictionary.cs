using System;
using System.Collections.Generic;

namespace Logger.ForEditorBaseScripts
{
    [Serializable]
    public enum QuestType {
        CollectItem = 0,
        ConsumeItem,
        BreakBlock,
        ConversationWithNpc,
        PlayerMoveDistance,
        PlayerMoveDeeply,
        EnterArea,
        KillMonster,
        EarnGold,
        ConsumeGold,
    }

    [Serializable]
    public enum TargetType
    {
        Player,     // need Player Actions or Situation
        Inventory,  // need to search Item
        World,      // need World object Information
        Entity,     // need Monster, NPC, etc Situation
        Area,       // need Collider, Trigger Action from special space
        Dialogue,   // need conversation with some Object ex.NPC
    }
    
    [Serializable]
    public class ConditionDictionary
    {
        private const string TypeInt = "int";
        private const string TypeFloat = "float";
        private const string TypeBool = "bool";
        
        public Dictionary<QuestType, ConditionType> typeDictionary = new Dictionary<QuestType, ConditionType>()
        {
            { QuestType.CollectItem, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = TypeInt
            }}, { QuestType.ConsumeItem, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = TypeInt
            }},{ QuestType.BreakBlock, new ConditionType
            {
                target = TargetType.World,
                typeValue = TypeInt
            }},{ QuestType.ConversationWithNpc, new ConditionType
            {
                target = TargetType.Dialogue,
                typeValue = TypeBool
            }},{ QuestType.PlayerMoveDistance, new ConditionType
            {
                target = TargetType.Player,
                typeValue = TypeFloat
            }},{ QuestType.PlayerMoveDeeply, new ConditionType
            {
                target = TargetType.Player,
                typeValue = TypeFloat
            }}, { QuestType.EnterArea, new ConditionType
            {
                target = TargetType.Area,
                typeValue = TypeBool
            }}, { QuestType.KillMonster, new ConditionType
            {
                target =  TargetType.Entity,
                typeValue = TypeInt
            }}, { QuestType.EarnGold, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = TypeInt
            }}, { QuestType.ConsumeGold, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = TypeInt
            }}
        };
    }
}