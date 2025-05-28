using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;

namespace Ciart.Pagomoa.Events
{
    public record LoadInventoryEvent(Inventory inventory) : IEvent;
    
    public record UpdateInventoryEvent(Inventory inventory) : IEvent;
    
    public record ItemCountChangedEvent(string itemID, int count) : IEvent;
    
    public record ItemUsedEvent(string itemID, int count) : IEvent;
    
    public record ItemSellEvent(string itemID, int count) : IEvent;
    
    public record AddReward(string itemID, int itemCount) : IEvent;
    
    public record AddGold(int gold) : IEvent;
}
