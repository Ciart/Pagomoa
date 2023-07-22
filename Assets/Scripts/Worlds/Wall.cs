using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worlds
{
    [CreateAssetMenu(fileName = "Wall", menuName = "World/Wall", order = 2)]
    public class Wall: ScriptableObject
    {
        public string displayName;

        public bool isClimbable;
        
        public Sprite sprite;

        public TileBase tile;
    }
}