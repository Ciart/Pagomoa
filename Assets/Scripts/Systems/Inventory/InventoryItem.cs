using System;
using Ciart.Pagomoa.Items;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class InventoryItem
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