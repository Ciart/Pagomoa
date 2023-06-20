using System;

namespace Worlds
{
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
    }
}