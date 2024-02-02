using System;

namespace Ciart.Pagomoa.Worlds
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
    public struct BrickHealth
    {
        public float health;
        public float maxHealth;
        public Brick brick;

        public BrickHealth(float health, float maxHealth, Brick brick)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.brick = brick;
        }

        public static BrickHealth FromBrick(Brick brick)
        {
            if (brick.ground is null)
            {
                return new BrickHealth(0, 0, brick);
            }

            return new BrickHealth(brick.ground.strength, brick.ground.strength, brick);
        }
    }

    [Serializable]
    public class Brick : ICloneable
    {
        public Wall wall;
        public Ground ground;
        public Mineral mineral;

        public bool isRock;

        public void CopyTo(Brick brick)
        {
            brick.wall = wall;
            brick.ground = ground;
            brick.mineral = mineral;
            brick.isRock = isRock;
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        public static bool operator ==(Brick lhs, Brick rhs) =>
            lhs.wall == rhs.wall && lhs.ground == rhs.ground && lhs.mineral == rhs.mineral &&
            lhs.mineral == rhs.mineral;

        public static bool operator !=(Brick lhs, Brick rhs) => !(lhs == rhs);
    }
}