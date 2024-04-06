using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logger.ForEditorBaseScripts
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class Quest : ScriptableObject
    {
        public bool clear = false;

        [SerializeField] public int questId;
        [SerializeField] public int nextQuestId;
        [SerializeField] public string title;
        [TextArea, SerializeField] public string description;
        [SerializeField] public Reward reward = new();
        
        public List<QuestCondition> questList;
    }

    [Serializable]
    public class Reward
    {
        public int gold;
        public ScriptableObject targetEntity;
        public Sprite targetEntitySprite;
        public int value;
    }
}