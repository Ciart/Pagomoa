using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Worlds;

namespace Logger
{
    public enum QuestType {
        CollectMineral = 0,
        ConsumeMineral,
        BreakBlock,
        EncounterMineral,
        EncounterNpc,
    }
    
    public class ConditionDictionary
    {
        public readonly Dictionary<QuestType, ConditionType> typeDictionary = new Dictionary<QuestType, ConditionType>()
        {
            { QuestType.CollectMineral, new ConditionType
            {
               TargetType = ScriptableObject.CreateInstance(typeof(Mineral)),
               Value = "int"
            }},
            { QuestType.ConsumeMineral, new ConditionType
            {
                TargetType = ScriptableObject.CreateInstance(typeof(Mineral)),
                Value = "int"
            }},{ QuestType.BreakBlock, new ConditionType
            {
                TargetType = ScriptableObject.CreateInstance(typeof(Brick)),
                Value = "int"
            }},{ QuestType.EncounterMineral, new ConditionType
            {
                TargetType = ScriptableObject.CreateInstance(typeof(Mineral)),
                Value = "bool"
            }},{ QuestType.EncounterNpc, new ConditionType
            {
                TargetType = ScriptableObject.CreateInstance(typeof(NPC)),
                Value = "bool"
            }}
        };
    }
}