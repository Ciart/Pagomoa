using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProgressedQuest
    {
        public int questId;
        public bool accomplishment;
        
        public ProgressedQuest(ProcessQuest quest)
        {
            questId = quest.questId;
            accomplishment = quest.accomplishment;
        }
    }
}