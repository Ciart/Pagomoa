using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;

namespace Ciart.Pagomoa.Events
{
    public record CollectItemEvent(InventoryItem item) : IEvent;
    
    public record ConsumeItemEvent(InventoryItem item) : IEvent;
}