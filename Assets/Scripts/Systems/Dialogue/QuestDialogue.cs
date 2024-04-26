using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    [CreateAssetMenu(fileName = "Quest Dialogue Data", menuName = "New Dialogue/Dialogue Quest Data", order = int.MaxValue)]
    public class QuestDialogue : ScriptableObject
    {
        public string questName;
        public int questId;
        public int[] questPrerequisiteIds;
        public TextAsset questStartPrologos;
        public TextAsset questCompletePrologos;
        public bool IsPrerequisiteCompleted()
        {
            // questPrerequisitelds Complete Check
            return true;
        }
    }
}
