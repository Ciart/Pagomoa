using System;
using System.Collections.Generic;

namespace Logger.ForEditorBaseScripts
{
    [Serializable]
    public enum QuestType {
        CollectMineral = 0,
        ConsumeMineral,
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
        private const string typeInt = "int";
        private const string typeFloat = "float";
        private const string typeBool = "bool";
        
        public Dictionary<QuestType, ConditionType> typeDictionary = new Dictionary<QuestType, ConditionType>()
        {
            { QuestType.CollectMineral, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = typeInt
            }}, { QuestType.ConsumeMineral, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = typeInt
            }},{ QuestType.BreakBlock, new ConditionType
            {
                target = TargetType.World,
                typeValue = typeInt
            }},{ QuestType.ConversationWithNpc, new ConditionType
            {
                target = TargetType.Dialogue,
                typeValue = typeBool
            }},{ QuestType.PlayerMoveDistance, new ConditionType
            {
                target = TargetType.Player,
                typeValue = typeFloat
            }},{ QuestType.PlayerMoveDeeply, new ConditionType
            {
                target = TargetType.Player,
                typeValue = typeFloat
            }}, { QuestType.EnterArea, new ConditionType
            {
                target = TargetType.Area,
                typeValue = typeBool
            }}, { QuestType.KillMonster, new ConditionType
            {
                target =  TargetType.Entity,
                typeValue = typeInt
            }}, { QuestType.EarnGold, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = typeInt
            }}, { QuestType.ConsumeGold, new ConditionType
            {
                target = TargetType.Inventory,
                typeValue = typeInt
            }}
        };
    }
}