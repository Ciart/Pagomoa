using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Wall
    {
        public string id;
        
        public string name;

        public bool isClimbable;
        
        public Sprite sprite;

        public TileBase tile;
        
        public void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Walls/{id}");
            tile = Resources.Load<TileBase>($"Walls/{id}");
        }
    }
}