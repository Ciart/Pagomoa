using System;
using Ciart.Pagomoa.Entities;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class WorldEntityData
    {
        public float x;

        public float y;

        public EntityOrigin origin;

        public EntityStatus status;

        public WorldEntityData(float x, float y, EntityOrigin origin, EntityStatus status = null)
        {
            this.x = x;
            this.y = y;
            this.origin = origin;
            this.status = status;
        }
    }
}
