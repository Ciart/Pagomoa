using System;

namespace Worlds
{
    [Serializable]
    public struct Brick
    {
        public static Brick None = new Brick();

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