namespace Ciart.Pagomoa.Events
{
    public record PausedEvent() : IEvent;
    
    public record ResumedEvent() : IEvent;
}
