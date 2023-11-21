using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Worlds;

namespace Logger
{
    [Serializable]
    public enum QuestType {
        CollectMineral = 0,
        ConsumeMineral,
        BreakBlock,
        EncounterMineral,
        EncounterNpc,
        Temp
    }

    public enum TargetType
    {
        Mineral,
        Brick,
        Npc,
    }
    
    [Serializable]
    public class ConditionDictionary
    {
        public Dictionary<QuestType, ConditionType> typeDictionary = new Dictionary<QuestType, ConditionType>()
        {
            { QuestType.CollectMineral, new ConditionType
            {
                Target = TargetType.Mineral,
                TypeValue = "int"
            }},
            { QuestType.ConsumeMineral, new ConditionType
            {
                Target = TargetType.Mineral,
                TypeValue = "int"
            }},{ QuestType.BreakBlock, new ConditionType
            {
                Target = TargetType.Brick,
                TypeValue = "int"
            }},{ QuestType.EncounterMineral, new ConditionType
            {
                Target = TargetType.Mineral,
                TypeValue = "bool"
            }},{ QuestType.EncounterNpc, new ConditionType
            {
                Target = TargetType.Npc,
                TypeValue = "bool"
            }},{ QuestType.Temp, new ConditionType
            {
                Target = TargetType.Npc,
                TypeValue = "float"
            }}
        };
    }
}