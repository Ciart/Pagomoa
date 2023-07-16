using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    [CreateAssetMenu(fileName = "Mineral", menuName = "Map/Mineral", order = 1)]
    public class Mineral: ScriptableObject
    {
        public string mineralName;
    
        public int tier;

        public Sprite sprite;
        
        public TileBase tile;

        public MineralItem item;

        public int price;
    }
}