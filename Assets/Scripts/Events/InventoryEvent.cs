using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;

namespace Ciart.Pagomoa.Events
{
    public record ItemCountEvent(Item item, int itemCount) : IEvent;
}