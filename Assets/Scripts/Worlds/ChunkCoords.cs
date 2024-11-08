using System;
using MemoryPack;

namespace Ciart.Pagomoa.Worlds
{
    [MemoryPackable]
    public struct ChunkCoords
    {
        public int x;
        public int y;

        public ChunkCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is ChunkCoords other)
            {
                return x == other.x && y == other.y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(ChunkCoords a, ChunkCoords b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ChunkCoords a, ChunkCoords b)
        {
            return !(a == b);
        }

        public static ChunkCoords operator +(ChunkCoords a, ChunkCoords b)
        {
            return new ChunkCoords(a.x + b.x, a.y + b.y);
        }
        
        public static ChunkCoords operator -(ChunkCoords a, ChunkCoords b)
        {
            return new ChunkCoords(a.x - b.x, a.y - b.y);
        }
    }
}