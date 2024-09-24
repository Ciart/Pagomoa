using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems.Dialogue;

namespace Ciart.Pagomoa.Events
{
    public record StoryStarted(DialogueManagement targetManagement = null) : IEvent;

    public record QuestStoryStarted(QuestData[] quests) : IEvent;
}