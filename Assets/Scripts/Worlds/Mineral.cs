using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worlds
{
    [CreateAssetMenu(fileName = "Mineral", menuName = "World/Mineral", order = 1)]
    public class Mineral: ScriptableObject
    {
        public string mineralName;
    
        public int tier;

        public Sprite sprite;
        
        public TileBase tile;

        public int price;
    }
}