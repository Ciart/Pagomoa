using System;
using System.Collections.Generic;
using Logger.ForEditorBaseScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Logger.ForEditorBaseScripts
{
    [CreateAssetMenu(menuName = "new Quest")]
    [Serializable]
    public class QuestData : ScriptableObject
    {
        public string id = "";
        public List<string> prevQuestIds = new();
        public string title = "";
        [TextArea] public string description = "";
        
        public TextAsset startPrologue = null;
        public TextAsset completePrologue = null;
        
        public Reward reward = new();
        
        public List<QuestConditionData> questList = new List<QuestConditionData>();
        public List<QuestData> prevQuestData = new List<QuestData>();
    }

    [Serializable]
    public class Reward
    {
        public int gold = 0;
        public string itemID = "";
        public Sprite itemSprite = null;
        public int value = 0;
    }
}
