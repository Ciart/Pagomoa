using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;

namespace Ciart.Pagomoa.Events
{
    public record UpdateInventory() : IEvent;
    public record ItemCountChangedEvent(string itemID, int count) : IEvent;
    
    public record ItemUsedEvent(string itemID, int count) : IEvent;
    
    public record AddReward(string itemID, int itemCount) : IEvent;
    
    public record AddGold(int gold) : IEvent;
}
