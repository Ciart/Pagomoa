using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Ground
    {
        public string id;
        
        public string name;
        
        public float strength;
        
        public string color;
        
        public Sprite sprite;

        public TileBase tile;
        
        public void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Grounds/{id}");
            tile = Resources.Load<TileBase>($"Grounds/{id}");
        }
    }
}