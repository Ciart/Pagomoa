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

        [field: SerializeField]
        public int width
        {
            get;
            private set;
        } = 2;

        [field: SerializeField]
        public int height
        {
            get;
            private set;
        } = 2;

        public Vector2Int pivot = Vector2Int.zero;

        public float weight = 1;

        public List<EntityData> entities = new();

        [SerializeField] private Brick[] _bricks;

        public bool isValid => _bricks.Length == width * height;

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            
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

        public void AddEntity(int x, int y, string entityId)
        {
            if (!CheckRange(x, y))
            {
                return;
            }

            var entityData = new EntityData(entityId, x, y);

            if (entities.Exists(data => data == entityData))
            {
                return;
            }
            
            entities.Add(entityData);
        }
    }
}