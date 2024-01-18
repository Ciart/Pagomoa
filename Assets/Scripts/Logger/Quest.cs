using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class Quest : ScriptableObject
    {
        public bool clear = false;

        [SerializeField] public int questId;
        [SerializeField] public int nextQuestId;
        [TextArea, SerializeField] public string description;
        [SerializeField] public Reward reward = new();
        
        public List<QuestCondition> questList;
    }

    [Serializable]
    public class Reward
    {
        public int gold;
        public ScriptableObject targetEntity;
        public int value;
    }
}