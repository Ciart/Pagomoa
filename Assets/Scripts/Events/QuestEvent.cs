namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;

    public record SignalToNpc(bool accomplishment) : IEvent;
}