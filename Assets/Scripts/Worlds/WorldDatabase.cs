using System;
using Ciart.Pagomoa.Entities;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Worlds
{
    [CreateAssetMenu(fileName = "World Database", menuName = "World/Database", order = 4)]
    public class WorldDatabase : ScriptableObject
    {
        public Entity[] entities;

        public Piece[] pieces = { new Piece() };

        public int selectIndex;

        public TileBase[] brokenEffectTiles;

        public TileBase glitterTile;

        public Piece GetPieceWithTag(string tag)
        {
            return Array.Find(pieces, piece => Array.Exists(piece.tags, t => t == tag));
        }
    }
}