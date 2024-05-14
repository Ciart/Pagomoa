using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logger.ForEditorBaseScripts
{
    [CreateAssetMenu(menuName = "MakeQuest")]
    public class Quest : ScriptableObject
    {
        public bool clear = false;

        [SerializeField] public GameObject questInCharge;
        [SerializeField] public int id;
        [SerializeField] public List<int> nextQuestIds = new();
        [SerializeField] public string title;
        [TextArea, SerializeField] public string description;
        
        [SerializeField] public TextAsset startPrologue;
        [SerializeField] public TextAsset completePrologue;
        
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