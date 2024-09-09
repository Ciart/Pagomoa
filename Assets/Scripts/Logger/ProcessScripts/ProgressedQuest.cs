using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public class ProgressedQuest
    {
        public string id;
        public bool accomplished;
        
        public ProgressedQuest(ProcessQuest quest)
        {
            id = quest.id;
            accomplished = quest.accomplished;
        } 
    }
}