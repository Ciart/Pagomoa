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

        public abstract bool TypeValidation(ScriptableObject target);
    }
    public interface IProcessQuestValue<T>
    {
        public T value { get; set; }
        public T compareValue { get; set; }
    }
}