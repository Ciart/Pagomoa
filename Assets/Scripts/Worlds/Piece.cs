using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    [Flags]
    public enum WorldAreaFlag
    {
        None = 0,
        Desert = 1 << 0,
        Forest = 1 << 1,
        All = ~0
    }

    [Serializable]
    public class Piece
    {
        public string name = "New Piece";
        
        public WorldAreaFlag appearanceArea = WorldAreaFlag.Desert;

        public string[] tags;

        public int width = 2;

        public int height = 2;

        public Vector2Int pivot = Vector2Int.zero;

        public float weight = 1;

        public List<EntityData> entities = new();

        [SerializeField] private Brick[] _bricks;

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks()
        {
            Array.Resize(ref _bricks, width * height);

            entities = entities.FindAll(item => CheckRange(item.x, item.y));
        }

        public Brick GetBrick(int x, int y)
        {
            return _bricks[GetBrickIndex(x, y)];
        }

        private int GetBrickIndex(int x, int y)
        {
            return x + y * width;
        }

        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < width && 0 <= y && y < height;
        }

        public void AddEntity(int x, int y, EntityOrigin origin)
        {
            if (!CheckRange(x, y))
            {
                return;
            }

            entities.Add(new EntityData(x, y, origin));
        }
    }
}