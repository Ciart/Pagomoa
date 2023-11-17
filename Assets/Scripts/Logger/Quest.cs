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
        
        public List<QuestCondition> questList = new List<QuestCondition>();
    }       
}