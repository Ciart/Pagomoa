using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;

namespace Ciart.Pagomoa.Events
{
    public record ItemCountChangedEvent(Item item, int count) : IEvent;
    
    public record ItemUsedEvent(Item item, int count) : IEvent;
    
    public record AddReward(Item item, int itemCount) : IEvent;
    
    public record AddGold(int gold) : IEvent;
    
    public record QuickSlotChangedEvent(int quickSlotID = -5, int dependentID = -5) : IEvent;
}
