using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Worlds
{
    [CreateAssetMenu(fileName = "Mineral", menuName = "World/Mineral", order = 1)]
    public class Mineral: ScriptableObject
    {
        public string displayName;
    
        public int tier;

        public Sprite sprite;
        
        public TileBase tile;

        public int price;
    }
}