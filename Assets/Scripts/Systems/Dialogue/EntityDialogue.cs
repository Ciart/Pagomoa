using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class EntityDialogue : MonoBehaviour
    {
        public TextAsset basicDialogue = null;

        public DailyDialogue dailyDialogues;

        public Quest[] entityQuests;

        private void Start()
        {
            for (int i = 0; i < entityQuests.Length; i++)
            {
                QuestManager.instance.database.quests.Add(entityQuests[i]);    
            }
        }
    }
}
