using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [CreateAssetMenu(fileName = "Mineral", menuName = "World/Mineral", order = 1)]
    public class Mineral: ScriptableObject
    {
        public string displayName;
    
        public int tier;

        public Sprite sprite;
        
        public TileBase tile;

        public Item item;
    }
}