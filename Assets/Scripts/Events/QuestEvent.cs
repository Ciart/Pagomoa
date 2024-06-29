using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;
    public record PlayerMove(Vector3 playerPos) : IEvent;
    public record SignalToNpc(string questId, bool accomplishment, InteractableObject questInCharge) : IEvent;
}