using Ink.Runtime;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public abstract class DialogueManagement : MonoBehaviour
    {
        public Story story;
        
        public virtual void StartStory(EntityDialogue dialogue, TextAsset storyText) {}
        
        public virtual void StartStory(TextAsset storyText) {}
        
        public virtual void StopStory() {}
    }
}