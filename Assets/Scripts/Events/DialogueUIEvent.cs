using UnityEngine;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;

namespace Ciart.Pagomoa.Events
{
    public record StoryStarted() : IEvent;

    public record QuestStoryStarted(Quest[] quests) : IEvent;
}