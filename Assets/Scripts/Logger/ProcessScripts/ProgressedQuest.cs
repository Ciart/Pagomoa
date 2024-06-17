using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProgressedQuest
    {
        public int id;
        public bool accomplishment;
        
        public ProgressedQuest(ProcessQuest quest)
        {
            id = quest.id;
            accomplishment = quest.accomplishment;
        }
    }
}