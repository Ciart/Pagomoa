using System;
using System.Collections.Generic;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ForEditorBaseScripts
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class QuestData : ScriptableObject
    {
        [SerializeField] public string id;
        [SerializeField] public List<string> prevQuestIds = new();
        [SerializeField] public string title;
        [TextArea, SerializeField] public string description;
        
        [SerializeField] public TextAsset startPrologue;
        [SerializeField] public TextAsset completePrologue;
        
        [SerializeField] public Reward reward = new();
        
        public List<QuestConditionData> questList = new List<QuestConditionData>();
        public List<QuestData> prevQuestData = new List<QuestData>();
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