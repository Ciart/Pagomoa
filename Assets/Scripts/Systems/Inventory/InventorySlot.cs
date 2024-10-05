using System;
using Ciart.Pagomoa.Items;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public struct InventorySlot
    {
        public Item item;
        public int count;
        public InventorySlot(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}