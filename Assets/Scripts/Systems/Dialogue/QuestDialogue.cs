using Ciart.Pagomoa.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    [CreateAssetMenu(fileName = "Quest Dialogue Data", menuName = "New Dialogue/Dialogue Quest Data", order = int.MaxValue)]
    public class QuestDialogue : ScriptableObject
    {
        public string questName;
        public int questId;
        public int[] questPrerequisiteIds;
        public TextAsset questStartPrologue;
        public TextAsset questCompletePrologue;
        public bool IsPrerequisiteCompleted()
        {
            bool isReceivable = true;
            foreach(var item in questPrerequisiteIds)
            {
               if(!QuestManager.instance.IsCompleteQuest(item)) isReceivable = false;
            }
            return isReceivable;
        }
    }
}
