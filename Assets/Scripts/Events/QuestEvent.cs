using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;
    public record SignalToNpc(string questID, bool accomplishment) : IEvent;
    public record CompleteQuestsUpdated(ProcessQuest[] completeQuests) : IEvent;

    public record PlayerMove(Vector3 playerPos) : IEvent;
}