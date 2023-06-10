using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    [CreateAssetMenu(fileName = "Ground", menuName = "Map/Ground", order = 2)]
    public class Ground: ScriptableObject
    {
        public string groundName;
        
        public float strength;
        
        public Sprite sprite;

        public TileBase tile;
    }
}