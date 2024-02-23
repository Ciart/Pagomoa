using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class ProcessQuestElements
    {
        public QuestType questType { get; set; }
        public string summary { get; set; }
        public ScriptableObject targetEntity { get; set; }
        public string valueType { get; set; }
        public bool complete { get; set; } = false;
        
        public abstract bool CheckComplete();

        public abstract bool TypeValidation(ScriptableObject target);
    }
}