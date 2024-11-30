using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Items
{
    public enum ItemType
    {
        Equipment,
        Use,
        Mineral,
        Inherent
    }

    public class ItemJsonData
    {
        public Item[] data;
    }
    
    [Serializable]
    public class Item
    {
        public ItemType type;
        
        public string id;
        
        public string name;
        
        public string description;

        public int price;

        public Sprite sprite;

        public void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Items/{id}");
        }
    }
}
