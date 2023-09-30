using System;

namespace Worlds
{
    [Serializable]
    public struct BrickCoords
    {
        public int x;
        public int y;
        
        public BrickCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    [Serializable]
    public class Brick
    {
        public Wall wall;
        public Ground ground;
        public Mineral mineral;

        public void CopyTo(Brick brick)
        {
            brick.wall = wall;
            brick.ground = ground;
            brick.mineral = mineral;
        }

        public static bool operator ==(Brick lhs, Brick rhs) =>
            lhs.wall == rhs.wall && lhs.ground == rhs.ground && lhs.mineral == rhs.mineral;

        public static bool operator !=(Brick lhs, Brick rhs) => !(lhs == rhs);
    }
}