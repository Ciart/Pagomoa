using System;
using Ciart.Pagomoa.Entities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [CreateAssetMenu(fileName = "World Database", menuName = "World/Database", order = 4)]
    public class WorldDatabase : ScriptableObject
    {
        public Wall[] walls;

        public Ground[] grounds;

        public Mineral[] minerals;

        public EntityOrigin[] entities;

        public Piece[] pieces = { new Piece() };

        public int selectIndex;

        public TileBase[] brokenEffectTiles;

        public TileBase glitterTile;
        public EntityOrigin GetEntity(string name)
        {
            return Array.Find(entities, entity => entity.name == name);
        }

        public Piece GetPieceWithTag(string tag)
        {
            return Array.Find(pieces, piece => Array.Exists(piece.tags, t => t == tag));
        }
    }
}