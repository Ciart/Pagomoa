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
                target = TargetType.Mineral,
                typeValue = "int"
            }},
            { QuestType.ConsumeMineral, new ConditionType
            {
                target = TargetType.Mineral,
                typeValue = "int"
            }},{ QuestType.BreakBlock, new ConditionType
            {
                target = TargetType.Brick,
                typeValue = "int"
            }},{ QuestType.EncounterMineral, new ConditionType
            {
                target = TargetType.Mineral,
                typeValue = "bool"
            }},{ QuestType.EncounterNpc, new ConditionType
            {
                target = TargetType.Npc,
                typeValue = "bool"
            }},{ QuestType.Temp, new ConditionType
            {
                target = TargetType.Npc,
                typeValue = "float"
            }}
        };
    }
}