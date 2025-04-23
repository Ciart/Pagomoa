using System;
using System.Collections.Generic;

namespace Ciart.Pagomoa.Worlds
{
    public struct WorldBounds
    {
        public int xMin;
        public int yMin;
        public int xMax;
        public int yMax;

        public WorldBounds(int xMin, int yMin, int xMax, int yMax)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
        }

        public WorldBounds(WorldCoords min, WorldCoords max)
        {
            xMin = min.x;
            yMin = min.y;
            xMax = max.x;
            yMax = max.y;
        }

        // TODO: 더 좋은 이름이 떠오르면 바꿉니다.
        public static WorldBounds FromTopBottomLeftRight(int top, int bottom, int left, int right)
        {
            return new WorldBounds(-left, -bottom, right, top);
        }

        public IEnumerable<ChunkCoords> GetChunkKeys()
        {
            var chunkTop = Math.Ceiling((float)yMax / Chunk.Size);
            var chunkBottom = Math.Floor((float)yMin / Chunk.Size);
            var chunkLeft = Math.Floor((float)xMin / Chunk.Size);
            var chunkRight = Math.Ceiling((float)xMax / Chunk.Size);
            
            for (var chunkX = chunkLeft; chunkX < chunkRight; chunkX++)
            {
                for (var chunkY = chunkBottom; chunkY < chunkTop; chunkY++)
                {
                    yield return new ChunkCoords((int)chunkX, (int)chunkY);
                }
            }
        }
        
        public IEnumerable<WorldCoords> GetWorldCoords()
        {
            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    yield return new WorldCoords(x, y);
                }
            }
        }
        
        public bool Contains(WorldCoords coords)
        {
            return coords.x >= xMin && coords.x <= xMax && coords.y >= yMin && coords.y <= yMax;
        }
    }
}
