using UnityEngine;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;

namespace Ciart.Pagomoa.Events
{
    public record RefreshView() : IEvent;

    public record UISetVisible(bool visible) : IEvent;

    public record UIMode(DialogueUI.UISelectMode mode) : IEvent;

    public record MakeQuestContentView(Quest[] quests) : IEvent;
}