using System;
using System.Resources;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Mineral
    {
        public string id;
        
        public string name;
    
        public int tier;

        public string itemId;

        public Sprite sprite;
        
        public TileBase tile;

        public Item item => ResourceSystem.instance.items[itemId];
        
        public void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Minerals/{id}");
            tile = Resources.Load<TileBase>($"Minerals/{id}");
        }
    }
}