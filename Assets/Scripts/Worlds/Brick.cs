using System;
using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Worlds
{
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
        public string wallId;
        public string groundId;
        public string mineralId;

        public Wall? wall
        {
            get
            {
                if (string.IsNullOrEmpty(wallId))
                    return null;
                
                return ResourceSystem.instance.GetWall(wallId);
            }
        }

        public Ground? ground
        {
            get
            {
                if (string.IsNullOrEmpty(groundId))
                    return null;

                return ResourceSystem.instance.GetGround(groundId); // 여기
            }
        }

        public Mineral? mineral
        {
            get
            {
                if (string.IsNullOrEmpty(mineralId))
                    return null;
                
                return ResourceSystem.instance.GetMineral(mineralId);
            }
        }

        public bool isRock;

        public void CopyTo(Brick brick)
        {
            brick.wallId = wallId;
            brick.groundId = groundId;
            brick.mineralId = mineralId;
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

        public override bool Equals(object obj) => obj is Brick other && this == other;

        public override int GetHashCode() => HashCode.Combine(wall, ground, mineral, isRock);
    }
}
