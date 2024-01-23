using Logger.ForEditorBaseScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Logger.ProcessScripts
{
    public abstract class ProcessQuestElements
    {
        public QuestType questType { get; set; }
        public string summary { get; set; }
        public ScriptableObject targetEntity { get; set; }
        public string valueType { get; set; }
        
        public abstract bool CheckComplete();
    }
}