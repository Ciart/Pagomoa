using Ciart.Pagomoa.Items;

namespace Ciart.Pagomoa.Events
{
    public record ItemCountEvent(Item item, int itemCount) : IEvent;
    
    public record AddReward(Item item, int itemCount) : IEvent;
    
    public record AddGold(int gold) : IEvent;
}