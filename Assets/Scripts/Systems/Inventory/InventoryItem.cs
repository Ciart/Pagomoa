using System;
using Ciart.Pagomoa.Items;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public struct InventoryItem
    {
        public Item item;
        public int count;
        public InventoryItem(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}