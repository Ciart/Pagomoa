using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class EntityDialogue : MonoBehaviour
    {
        public TextAsset basicDialogue = null;

        public DailyDialogue dailyDialogues;

        public Quest[] entityQuests;
    }
}
