using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Logger
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class Quest<T> : ScriptableObject
    {
        public bool clear = false;
        [Space]
        public int questId;
        public int nextQuestId;
        [TextArea] public string description;
        // public bool active;

        public Dictionary<string, string> questStringDic = new Dictionary<string, string>();
        public Dictionary<string, bool> questBoolDic = new Dictionary<string, bool>();
        public Dictionary<string, int> questIntDic = new Dictionary<string, int>();
        public Dictionary<string, float> questFloatDic = new Dictionary<string, float>();
    }
}
