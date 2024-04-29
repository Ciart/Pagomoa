using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;

    public record SignalToNpc(int questId, bool accomplishment, InteractableObject questInCharge) : IEvent;

    public record PlayerMove(Vector3 playerPos) : IEvent;
    
    public record QuestCompleted(InteractableObject questInCharge) : IEvent;
}