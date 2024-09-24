using System;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class ProcessQuestElements
    {
        public float progress;
        
        public Action questFinished;
        public QuestType questType { get; set; }
        public string summary { get; set; }
        public ScriptableObject targetEntity { get; set; }
        public string valueType { get; set; }

        public int value { get; set; }
        public int compareValue { get; set; }
        
        public abstract bool TypeValidation(ScriptableObject target);
    }
}