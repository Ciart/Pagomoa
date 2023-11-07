using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Logger
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class Quest : ScriptableObject
    {
        public bool clear = false;
        [Space]
        public int questId;
        public int nextQuestId;
        [TextArea] public string description;
        // public bool active;

        public List<QuestType<int>> questIntList = new List<QuestType<int>>();
        public List<QuestType<float>> questFloatList = new List<QuestType<float>>();
        public List<QuestType<bool>> questBoolList = new List<QuestType<bool>>();
    }
}