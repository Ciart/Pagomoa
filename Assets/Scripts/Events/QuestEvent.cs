using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;

    public record SignalToNpc(int questId, bool accomplishment, InteractableObject questInCharge) : IEvent;

    public record QuestCompleted(InteractableObject questInCharge) : IEvent;
}