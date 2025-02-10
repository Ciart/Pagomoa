using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Events
{
    public record GameStateChangedEvent(GameState state) : IEvent;
}
