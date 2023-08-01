using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Worlds
{
    [CreateAssetMenu(fileName = "Ground", menuName = "World/Ground", order = 2)]
    public class Ground: ScriptableObject
    {
        public string displayName;
        
        public float strength;
        
        public Sprite sprite;

        public TileBase tile;
    }
}